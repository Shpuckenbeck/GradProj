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
using System.IO;
using System.Text;
using System.Security.Claims;
using System.Diagnostics;


namespace Grad.Controllers
{
    [Authorize(Roles = "user")]
    public class StatesController : Controller
    {
        private readonly EditorContext _context;

        public StatesController(EditorContext context)
        {
            _context = context;    
        }

        // GET: States
        public async Task<IActionResult> Index()
        {
            var editorContext = _context.States.Include(s => s.Article).Include(s => s.Status);
            return View(await editorContext.ToListAsync());
        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .Include(s => s.Article)
                .Include(s => s.Status)
                .SingleOrDefaultAsync(m => m.StateId == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr");
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName");
            DateTime test = DateTime.Now;
          ViewData["StateDate"] = test;
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StateId,StatusId,StateDescr,StateDate,ArticleId")] State state)
        {
            if (ModelState.IsValid)
            {
                _context.Add(state);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", state.ArticleId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", state.StatusId);
            return View(state);
        }
        [Authorize(Roles = "editor")]
        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States.SingleOrDefaultAsync(m => m.StateId == id);
            if (state == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", state.ArticleId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", state.StatusId);
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StateId,StatusId,StateDescr,StateDate,ArticleId")] State state)
        {
            if (id != state.StateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.StateId))
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
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleID", "ArtDescr", state.ArticleId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", state.StatusId);
            return View(state);
        }
        [Authorize(Roles = "editor")]
        // GET: States/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .Include(s => s.Article)
                .Include(s => s.Status)
                .SingleOrDefaultAsync(m => m.StateId == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var state = await _context.States.SingleOrDefaultAsync(m => m.StateId == id);
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Add(int id)
        {
            AddStateViewModel model = new AddStateViewModel();
            DateTime my = DateTime.Now;
            model.date = my;
            model.artid = id;
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            model.name = article.ArtName;
            ViewData["status"] = new SelectList(_context.Status, "StatusId", "StatusName");
            return View(model);
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddStateViewModel model)
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
                if ((r.Value == "admin") || (r.Value == "editor"))
                {
                    editorflag = true;
                    break;
                }
            }
            if (ModelState.IsValid)
            {
                State state = new State();
                state.StatusId = model.status;
                state.ArticleId = model.artid;
                state.StateDescr = model.descr;
                state.StateDate = model.date;
                _context.Add(state);
                await _context.SaveChangesAsync();
                if (editorflag == true)
                {
                    string path = "/Notes/Add/" + model.artid.ToString();
                    return Redirect(path);
                }
                else
                return RedirectToAction("Index", "Articles");
            }
            return RedirectToAction("Index");
        }

        private bool StateExists(int id)
        {
            return _context.States.Any(e => e.StateId == id);
        }
    }
}
