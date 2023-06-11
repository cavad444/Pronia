using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    public class TagController : Controller
    {
        private readonly ProniaDbContext _context;
        public TagController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Tags.Include(x => x.PlantTags).AsQueryable();
            return View(CustomPaginatedList<Tag>.CreateCustomList(query, page, 2));
        }
        public IActionResult Create()
        { 
         return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Tags.Any(x=>x.Name==tag.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.Find(id);
            if(tag==null)
            {
                return View("Error");
            }
            return View(tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag tag)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            Tag existTag=_context.Tags.Find(tag.Id);
            if (existTag == null)
            {
                return View("Error");
            }
            if (tag.Name!=existTag.Name && _context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }
            existTag.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction("index");

        }
        public IActionResult Delete(int id)
        {
            Tag tag=_context.Tags.Find(id);
            if(tag==null)
            {
                return StatusCode(404);
            }
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return StatusCode(200);
        }
    }
}
