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

        private string Convert(string source)
        {
            string processName = @"C:\Users\Jimmy\Appdata\Local\Pandoc\pandoc.exe";
            string args = String.Format(@"-r html -t docx");

            ProcessStartInfo psi = new ProcessStartInfo(processName, args);

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;

            Process p = new Process();
            p.StartInfo = psi;
            psi.UseShellExecute = false;
            p.Start();

            string outputString = "";
            byte[] inputBuffer = Encoding.UTF8.GetBytes(source);
            p.StandardInput.BaseStream.Write(inputBuffer, 0, inputBuffer.Length);
            //p.StandardInput.Close();
            

            p.WaitForExit(2000);
            using (System.IO.StreamReader sr = new System.IO.StreamReader(
                                                   p.StandardOutput.BaseStream))
            {

                outputString = sr.ReadToEnd();
            }

            return outputString;
        }
        [Authorize(Roles = "editor")]
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
        [Authorize(Roles = "editor")]
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
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Blocked()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Write(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            bool editorflag = false;
            foreach (var r in roles)
            {
              if( (r.Value=="admin")|| (r.Value == "editor"))
                {
                    editorflag = true;
                    break;
                }
            }
            if (editorflag == true)
            {
                TextEditViewModel edmod = new TextEditViewModel();
                edmod.artid = id;
                var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == edmod.artid);

                if ((article.blocked == null) || (article.blocked == currentUserID))
                {

                    edmod.Name = article.ArtName;
                    edmod.Text = article.content;
                    ViewData["notes"] = new MultiSelectList(_context.Notes.Where(m => (m.ArticleId == id) && (m.Fixed == false)), "NoteId", "NoteDescr");
                    if (article.blocked == null)
                    {
                        article.blocked = currentUserID;
                        _context.Update(article);
                        await _context.SaveChangesAsync();
                    }
                    return View(edmod);
                }
                else return RedirectToAction("Blocked");
            }
            else
            {
                var author = await _context.Authors.SingleOrDefaultAsync(m => (m.ArticleID == id) && (m.UserId == currentUserID));
                if (author == null)
                {
                    

                    return RedirectToAction("AccessDenied");

                }
                else
                {
                    

                    TextEditViewModel edmod = new TextEditViewModel();
                    edmod.artid = id;
                    var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == edmod.artid);
                    if ((article.blocked == null) || (article.blocked == currentUserID))
                    {

                        edmod.Name = article.ArtName;
                    edmod.Text = article.content;
                    ViewData["notes"] = new MultiSelectList(_context.Notes.Where(m => (m.ArticleId == id) && (m.Fixed == false)), "NoteId", "NoteDescr");
                        if (article.blocked == null)
                        {
                            article.blocked = currentUserID;
                            _context.Update(article);
                            await _context.SaveChangesAsync();
                        }
                        return View(edmod);
                    }
                    else return RedirectToAction("Blocked");
                }
            }
        }

        [HttpPost]

        public async Task<ActionResult> Write(int id, TextEditViewModel model)
        {

           

            if (ModelState.IsValid)
            {
                var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
                article.content = model.Text;
                article.blocked = null;
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
            string path = "/States/Add/" + id.ToString();
            return Redirect(path);
        }

        [Authorize(Roles = "editor")]
        public async Task<IActionResult> Unblock(int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(Article model, int id)
        {
            
             await Open(id);
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Open (int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            article.blocked = null;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            

            return View();
        }

        //public IActionResult Export(int id)
        //{
        //    ExportViewModel model = new ExportViewModel();
        //    model.artid = id;
        //    return View(model);
        //}

        //// POST: Articles/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Export(ExportViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string path = @"c:\temp\" + model.filename + ".docx";
        //        Encoding test = Encoding.UTF8;
        //        var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == model.artid);
        //        Article testart = new Article();
        //        string scr = article.content;
        //        string output = Convert(scr);
        //        //testart.ArtName = "Debug";
        //        //testart.ArtDescr = "test";
        //        //testart.Deadline = DateTime.Now;
        //        //testart.content = output;
        //        //_context.Articles.Add(testart);
        //        //await _context.SaveChangesAsync();
        //        using (FileStream file1 = new FileStream(path, FileMode.Create))
        //        {
        //            byte[] record = test.GetBytes(output);
        //            await file1.WriteAsync(record, 0, record.Length);

        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
