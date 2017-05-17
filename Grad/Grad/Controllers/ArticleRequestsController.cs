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
using Microsoft.AspNetCore.Identity;

namespace Grad.Controllers
{
    [Authorize(Roles ="user")]
    public class ArticleRequestsController : Controller
    {
        private readonly EditorContext _context;

        public ArticleRequestsController(EditorContext context)
        {
            _context = context;    
        }

        // GET: ArticleRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticleRequests.ToListAsync());
        }

        // GET: ArticleRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests
                .SingleOrDefaultAsync(m => m.ArticleRequestId == id);
            if (articleRequest == null)
            {
                return NotFound();
            }

            return View(articleRequest);
        }

        // GET: ArticleRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArticleRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleRequestId,ReqName,ReqDescr,ReqDate")] ArticleRequest articleRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(articleRequest);
        }

        // GET: ArticleRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests.SingleOrDefaultAsync(m => m.ArticleRequestId == id);
            if (articleRequest == null)
            {
                return NotFound();
            }
            return View(articleRequest);
        }

        // POST: ArticleRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleRequestId,ReqName,ReqDescr,ReqDate")] ArticleRequest articleRequest)
        {
            if (id != articleRequest.ArticleRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleRequestExists(articleRequest.ArticleRequestId))
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
            return View(articleRequest);
        }

        // GET: ArticleRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests
                .SingleOrDefaultAsync(m => m.ArticleRequestId == id);
            if (articleRequest == null)
            {
                return NotFound();
            }

            return View(articleRequest);
        }

        // POST: ArticleRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleRequest = await _context.ArticleRequests.SingleOrDefaultAsync(m => m.ArticleRequestId == id);
            _context.ArticleRequests.Remove(articleRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ArticleRequestExists(int id)
        {
            return _context.ArticleRequests.Any(e => e.ArticleRequestId == id);
        }
        //-------------------------Регистрация статьи (работает)----------------------------
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> Register(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleRequest = await _context.ArticleRequests.SingleOrDefaultAsync(m => m.ArticleRequestId == id);
            if (articleRequest == null)
            {
                return NotFound();
            }
            return View(articleRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int id, [Bind("ArticleRequestId,ReqName,ReqDescr,ReqDate")] ArticleRequest articleRequest)
        {
            if (id != articleRequest.ArticleRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Article art = new Article();
                    art.ArtDescr = articleRequest.ReqDescr;
                    art.ArtName = articleRequest.ReqName;
                    art.Deadline = articleRequest.ReqDate;
                    _context.Articles.Add(art);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) //базарю, половину этого кода наверняка можно выкинуть. но кого это волнует? явно не меня-из-настоящего
                {
                    if (!ArticleRequestExists(articleRequest.ArticleRequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            _context.ArticleRequests.Remove(articleRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Articles");

        }

    }
}
