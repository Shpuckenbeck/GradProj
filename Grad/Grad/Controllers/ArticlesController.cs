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
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            TextEditViewModel edmod = new TextEditViewModel();
            if (article.FileName == null)
                {
                //edmod.Title = article.ArtName;
                //edmod.Description = article.ArtDescr;
            }

            else 
            {
               
                string path = @"c:\temp\"+article.FileName + ".docx";
                Encoding test = Encoding.UTF8;
                using (FileStream file1 = new FileStream(path, FileMode.Open))
                {
                    byte[] filebytes = new byte[file1.Length];
                   await file1.ReadAsync(filebytes, 0, Convert.ToInt32(file1.Length)); //await?
                    edmod.Text = test.GetString(filebytes);
                    edmod.Name = article.FileName;
                }
                // Encoding enc = Encoding.UTF8;

                //using (StreamReader sr = new StreamReader(file1, System.Text.Encoding.UTF8))
                //{
                //   byte[] 

                //}
            


            }
            edmod.artid = article.ArticleID;
            return View(edmod);
        }

        [HttpPost]
        public async Task<ActionResult> Write(TextEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = @"c:\temp\" + model.Name + ".docx";
                Encoding test = Encoding.UTF8;
                using (FileStream file1 = new FileStream(path, FileMode.Create))
                {
                    byte[] record = test.GetBytes(model.Text);
                    await file1.WriteAsync(record, 0, record.Length);

                }
                //using (StreamWriter sw = new StreamWriter(file1))
                //{
                //    //sw.WriteLine(model.Title);
                //    //sw.WriteLine(model.Description);
                //    //sw.Write(model.Text);
                    
                //}
               //var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == model.artid);
               //article.FileName = model.Name;
               // _context.Update(article);

             
            }
            return View();
        }
    }
}
