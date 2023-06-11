using Pronia.DAL;
using Pronia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly ProniaDbContext _context;
        public CategoryController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.Include(x=>x.Plants).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Categories.Any(x=>x.Name==category.Name))
            {
                ModelState.AddModelError("Name", "Name has already taken");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return StatusCode(404);
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return StatusCode(200);
        }
    }
}
