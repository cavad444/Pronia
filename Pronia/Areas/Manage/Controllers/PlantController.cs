using Pronia.DAL;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinal.Areas.Manage.Controllers
{
    [Area("manage")]
    public class PlantController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantController(ProniaDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1,string search=null)
        {
            var query = _context.Plants.Include(x => x.Category).Include(x=>x.PlantImages.Where(bi=>bi.PosterStatus==true)).AsQueryable();
            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
            ViewBag.Search=search;
            return View(CustomPaginatedList<Plant>.CreateCustomList(query,page,3));
        }
        public IActionResult Create()
        {
            ViewBag.Categories=_context.Categories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!_context.Categories.Any(x => x.Id == plant.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category id is not correct");
                return View();
            }

            if (plant.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage", "Poster image is required");
                return View();
            }
            if (plant.HoverPosterImage == null)
            {
                ModelState.AddModelError("HoverPosterImage", "Poster image is required");
                return View();
            }
            PlantImage poster = new PlantImage
            {
                ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", plant.PosterImage),
                PosterStatus = true,
            };
            plant.PlantImages.Add(poster);

            PlantImage hoverPoster = new PlantImage
            {
                ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", plant.HoverPosterImage),
                PosterStatus = false,
            };
            plant.PlantImages.Add(hoverPoster);
            foreach (var img in plant.Images)
            {
                PlantImage plantImage = new PlantImage
                {
                    ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", img),
                };
                plant.PlantImages.Add(plantImage);
            }
            _context.Plants.Add(plant);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();


            Plant plant = _context.Plants.Include(x => x.PlantImages).Include(x => x.PlantTags).FirstOrDefault(x => x.Id == id);

            return View(plant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Plant plant)
        {
            if (!ModelState.IsValid) return View();

            Plant existPlant = _context.Plants.Include(x => x.PlantTags).Include(x => x.PlantImages).FirstOrDefault(x => x.Id == plant.Id);

            if (existPlant == null) return View("Error");

            if (plant.CategoryId != existPlant.CategoryId && !_context.Categories.Any(x => x.Id == plant.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category Is is not correct");
                return View();
            }


            string oldPoster = null;
            if (plant.PosterImage != null)
            {
                PlantImage poster = existPlant.PlantImages.FirstOrDefault(x => x.PosterStatus == true);
                oldPoster = poster?.ImageName;

                if (poster == null)
                {
                    poster = new PlantImage { PosterStatus = true };
                    poster.ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", plant.PosterImage);
                    existPlant.PlantImages.Add(poster);
                }
                else
                    poster.ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", plant.PosterImage);
            }

            string oldHoverPoster = null;
            if (plant.HoverPosterImage != null)
            {
                PlantImage hoverPoster = existPlant.PlantImages.FirstOrDefault(x => x.PosterStatus == false);
                oldHoverPoster = hoverPoster?.ImageName;

                if (hoverPoster == null)
                {
                    hoverPoster = new PlantImage { PosterStatus = false };
                    hoverPoster.ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", plant.HoverPosterImage);
                    existPlant.PlantImages.Add(hoverPoster);
                }
                else
                    hoverPoster.ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", plant.HoverPosterImage);
            }

            var removedImages = existPlant.PlantImages.FindAll(x => x.PosterStatus == null);
            existPlant.PlantImages.RemoveAll(x => x.PosterStatus == null);

            foreach (var item in plant.Images)
            {
                PlantImage bookImage = new PlantImage
                {
                    ImageName = CustomFileManager.SaveFile(_env.WebRootPath, "uploads/plants", item),
                };
                existPlant.PlantImages.Add(bookImage);
            }

            existPlant.Name = plant.Name;
            existPlant.SalePrice = plant.SalePrice;
            existPlant.CostPrice = plant.CostPrice;
            existPlant.Description = plant.Description;
            existPlant.IsFeatured = plant.IsFeatured;
            existPlant.IsFeatured = plant.IsFeatured;
            existPlant.BestSeller = plant.BestSeller;
            existPlant.Latest = plant.Latest;
            existPlant.CategoryId = plant.CategoryId;

            _context.SaveChanges();


            if (oldPoster != null) CustomFileManager.DeleteFile(_env.WebRootPath, "uploads/plants", oldPoster);
            if (oldHoverPoster != null) CustomFileManager.DeleteFile(_env.WebRootPath, "uploads/plants", oldHoverPoster);

            if (removedImages.Any())
                CustomFileManager.DeleteAllFiles(_env.WebRootPath, "uploads/plants", removedImages.Select(x => x.ImageName).ToList());


            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Plant plant = _context.Plants.Find(id);
            if (plant == null)
            {
                return StatusCode(404);
            }
            _context.Plants.Remove(plant);
            _context.SaveChanges();
            return StatusCode(200);
        }
    }
}
