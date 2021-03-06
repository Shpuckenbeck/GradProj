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
    [Authorize(Roles = "editor")]
    public class AuthorsController : Controller
    {
        private readonly EditorContext _context;

        public AuthorsController(EditorContext context)
        {
            _context = context;    
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var editorContext = _context.Authors.Include(a => a.Article).Include(a => a.User);
            return View(await editorContext.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Article)
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "displayedname");
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,UserId,ArticleID")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", author.ArticleID);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "displayedname", author.UserId);
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.SingleOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", author.ArticleID);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "displayedname", author.UserId);
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,UserId,ArticleID")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", author.ArticleID);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "displayedname", author.UserId);
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Article)
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.SingleOrDefaultAsync(m => m.AuthorId == id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Add(int id)
        {
            AddAuthViewModel model = new AddAuthViewModel();
            model.artid = id;
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            model.name = article.ArtName;
            ViewData["names"] = new MultiSelectList(_context.User, "Id", "displayedname");
            return View(model);
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddAuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (string test in model.authors)
                {
                    Author auth = new Author();
                    auth.ArticleID = model.artid;
                    auth.UserId = test;
                    _context.Add(auth);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
            
        

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}
