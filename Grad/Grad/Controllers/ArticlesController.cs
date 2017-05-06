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

namespace Grad.Controllers
{
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
                edmod.Title = article.ArtName;
                edmod.Description = article.ArtDescr;
            }

            else //открытие файла с разбиением на абзацы
            {
               
                string path = @"c:\temp\"+article.FileName + ".doc";
                FileStream file1 = new FileStream(path, FileMode.Open);
               // Encoding enc = Encoding.UTF8;
                using (StreamReader sr = new StreamReader(file1, System.Text.Encoding.UTF8))
                {
                    edmod.Title = sr.ReadLine();
                    edmod.Description = sr.ReadLine();
                    //edmod.Text = enc.GetBytes(sr.ReadToEnd());
                    edmod.Text = sr.ReadToEnd();
                    
                }

               
            }
            edmod.artid = article.ArticleID;
            return View(edmod);
        }

        [HttpPost]
        public async Task<ActionResult> Write(TextEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = @"c:\temp\" + model.Name + ".doc";
                FileStream file1 = new FileStream(path, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(file1))
                {
                    sw.WriteLine(model.Title);
                    sw.WriteLine(model.Description);
                    sw.Write(model.Text);
                    
                }
               //var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == model.artid);
               //article.FileName = model.Name;

                //User user = new User { UserName = model.Login, Email = model.Email, PhoneNumber = model.Phone, Name = model.Name, Surname = model.Surname };
                //IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                //if (result.Succeeded)
                //{
                //    return RedirectToAction("UserList", "Roles");
                //}
                //else
                //    AddErrors(result);
            }
            return View();
        }
    }
}
