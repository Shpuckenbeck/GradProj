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

namespace Grad.Controllers
{
    [Authorize(Roles ="user")]
    public class ArticlesController : Controller
    {
        private readonly EditorContext _context;

        public ArticlesController(EditorContext context)
        {
            _context = context;    
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .SingleOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleID,ArtName,ArtDescr,Deadline")] Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,ArtName,ArtDescr,Deadline")] Article article)
        {
            if (id != article.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleID))
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
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .SingleOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }

        [HttpGet]
        public async Task<IActionResult> Write(int id)
        {
            TextEditViewModel edmod = new TextEditViewModel();
            edmod.artid = id;
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == edmod.artid);                 
           
            edmod.Name = article.ArtName;     
            edmod.Text = article.content;
            ViewData["notes"] = new MultiSelectList(_context.Notes.Where(m=>m.ArticleId==id), "NoteId", "NoteDescr");
            return View(edmod);
        }

        [HttpPost]

        public async Task<ActionResult> Write(TextEditViewModel model)
        {

           

            if (ModelState.IsValid)
            {
                var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == model.artid);
                article.content = model.Text;               
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                if (model.notes!=null)
                {
                    foreach (int i in model.notes)
                    {
                        var note = await _context.Notes.SingleOrDefaultAsync(m => m.NoteId == i);
                        note.Fixed = true;
                        _context.Notes.Update(note);
                        await _context.SaveChangesAsync();

                    }
                }



            }
            return View();
        }
    }
}
