using Pronia.DAL;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinal.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(ProniaDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            var query= _context.Sliders.OrderBy(x=>x.Order).AsQueryable();
            return View(CustomPaginatedList<Slider>.CreateCustomList(query, page, 2));
        }
        public IActionResult Create()
        {
            ViewBag.NextOrder = _context.Sliders.Any() ? _context.Sliders.Max(x => x.Order) + 1 : 1;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            ViewBag.NextOrder = slider.Order;
            if(!ModelState.IsValid)
            {
                return View();
            }
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Required");
                return View();
            }
            foreach(var item in _context.Sliders.Where(x=>x.Order>=slider.Order))
            {
                item.Order++;
            }
            string path = Path.Combine(_env.WebRootPath, "uploads/sliders", slider.ImageFile.FileName);
           
            slider.ImageName=CustomFileManager.SaveFile(_env.WebRootPath,"uploads/sliders",slider.ImageFile);
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null)
            {
                return View("Error");
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existSlider = _context.Sliders.Find(slider.Id);

            if (existSlider == null) return View("Error");

            existSlider.Order = slider.Order;
            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.BtnText = slider.BtnText;
            existSlider.Desc = slider.Desc;

            string oldFileName = null;
            if (slider.ImageFile != null)
            {
                oldFileName = existSlider.ImageName;
                existSlider.ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }

            _context.SaveChanges();

            if (oldFileName != null)
                CustomFileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", oldFileName);

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null)
            {
                return StatusCode(404);
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return StatusCode(200);
        }
    }
}
