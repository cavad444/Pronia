using Pronia.DAL;
using Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProniaDbContext _context;
        public HomeController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel
            {
                FeaturedPlant = _context.Plants.Include(x => x.PlantImages).Where(x => x.IsFeatured).Take(8).ToList(),
                BestSellerPlant = _context.Plants.Include(x => x.PlantImages).Where(x => x.BestSeller).Take(8).ToList(),
                LatestPlant = _context.Plants.Include(x => x.PlantImages).Where(x => x.Latest).Take(8).ToList(),


            };
            return View(vm);
        }
        public IActionResult SetSession()
        {
            HttpContext.Session.SetString("plantCount", "20");
            return RedirectToAction("index");
        }
        public IActionResult GetSession()
        {
            var val = HttpContext.Session.GetString("plantCount");
            return Json(new { plantCount = val });
        }
        public IActionResult SetCookie()
        {
            Response.Cookies.Append("plantCount", "30");
            return RedirectToAction("index");
        }
        public IActionResult GetCookie()
        {
            var val = Request.Cookies["plantCount"];
            return Json(new { plantCount = val });
        }
    }
}
