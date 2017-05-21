using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grad.Data;
using Grad.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Diagnostics;

namespace Grad.Controllers
{
    public class PicturesController : Controller
    {
        private readonly EditorContext _context;
        IHostingEnvironment _appEnvironment;

        public PicturesController(EditorContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.Pictures.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                DateTime my = DateTime.Now;
                Picture file = new Picture { Name = uploadedFile.FileName, Path = path, uploaddate = my };
                _context.Pictures.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            foreach (var uploadedFile in uploads)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                DateTime my = DateTime.Now;
                Picture file = new Picture { Name = uploadedFile.FileName, Path = path, uploaddate = my };
                _context.Pictures.Add(file);
            }
            _context.SaveChanges();

            return RedirectToAction("AddFile");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picRequest = await _context.Pictures
                .SingleOrDefaultAsync(m => m.PictureId == id);
            if (picRequest == null)
            {
                return NotFound();
            }

            return View(picRequest);
        }
    }
}