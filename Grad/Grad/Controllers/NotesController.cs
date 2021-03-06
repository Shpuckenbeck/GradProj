using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grad.Data;
using Grad.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Diagnostics;

namespace Grad.Controllers
{
    [Authorize(Roles = "editor")]
    public class NotesController : Controller
    {
        private readonly EditorContext _context;

        public NotesController(EditorContext context)
        {
            _context = context;    
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var editorContext = _context.Notes.Include(n => n.Article);
            return View(await editorContext.ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Article)
                .SingleOrDefaultAsync(m => m.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        [Authorize(Roles = "editor")]
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteId,ArticleId,NoteDescr,NoteDate")] Note note)
        {
            if (ModelState.IsValid)
            {
                note.Fixed = false;
                note.Checked = false;
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", note.ArticleId);
            return View(note);
        }
        [Authorize(Roles = "editor")]
        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.SingleOrDefaultAsync(m => m.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", note.ArticleId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteId,ArticleId,NoteDescr,NoteDate,Fixed,Checked")] Note note)
        {
            if (id != note.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.NoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", note.ArticleId);
            return View(note);
        }
        [Authorize(Roles = "editor")]
        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Article)
                .SingleOrDefaultAsync(m => m.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.SingleOrDefaultAsync(m => m.NoteId == id);
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "editor")]
        public IActionResult Add(int id)
        {
            Note model = new Note();
            DateTime my = DateTime.Now;
            model.NoteDate = my;
            model.ArticleId = id;
            return View(model);
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Note model)
        {
            if (ModelState.IsValid)
            {
                model.Checked = false;
                model.Fixed = false;
                _context.Add(model);
                await _context.SaveChangesAsync();
                Note model1 = new Note();
                DateTime my = DateTime.Now;
                model1.NoteDate = my;
                model1.NoteDescr = "";
                model1.ArticleId = model.ArticleId;
                return View(model1);
            }
            return RedirectToAction("Index");
        }
       
        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.NoteId == id);
        }
    }
}
