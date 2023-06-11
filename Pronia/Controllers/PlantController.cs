using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackendFinal.Controllers
{
    public class PlantController : Controller
    {
        public readonly ProniaDbContext _context;
        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult GetPlantDetail(int id)
        {
            Plant plant = _context.Plants.Include(x => x.PlantImages).Include(x => x.PlantTags).ThenInclude(x => x.Tag).FirstOrDefault(x => x.Id == id);
            if (plant == null) return StatusCode(404);
            return PartialView("_PlantModalPartial", plant);
        }
        public IActionResult AddToBasket(int id)
        {
            List<BasketProductCookieViewModel> cookieItems = new List<BasketProductCookieViewModel>();
            BasketProductCookieViewModel cookieItem;
            var baskerStr = Request.Cookies["basket"];
            if (baskerStr != null)
            {
                cookieItems = JsonConvert.DeserializeObject<List<BasketProductCookieViewModel>>(baskerStr);
                cookieItem = cookieItems.FirstOrDefault(x => x.PlantId == id);
                if (cookieItem != null)
                {
                    cookieItem.Count++;
                }
                else
                {
                    cookieItem = new BasketProductCookieViewModel { PlantId = id, Count = 1 };
                    cookieItems.Add(cookieItem);
                }
            }
            else
            {
                cookieItem = new BasketProductCookieViewModel { PlantId = id, Count = 1 };
                cookieItems.Add(cookieItem);
            }
            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));
            BasketViewModel bv = new BasketViewModel();
            foreach (var ci in cookieItems)
            {
                BasketProductViewModel bi = new BasketProductViewModel
                {
                    Count = ci.Count,
                    Plant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == ci.PlantId)
                };
                bv.BasketProducts.Add(bi);
                bv.TotalPrice += bi.Plant.SalePrice * bi.Count;
            }
            return PartialView("_BasketPartialView", bv);
        }
        public IActionResult ShowBasket()
        {
            var basket = new List<BasketProductCookieViewModel>();
            var basketStr = Request.Cookies["basket"];
            if (basketStr != null)
                basket = JsonConvert.DeserializeObject<List<BasketProductCookieViewModel>>(basketStr);
            return Json(new { basket });
        }
        public IActionResult RemoveBasket(int id)
        {
            var basketStr = Request.Cookies["basket"];
            if (basketStr == null)
                return StatusCode(404);

            List<BasketProductCookieViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketProductCookieViewModel>>(basketStr);

            BasketProductCookieViewModel item = cookieItems.FirstOrDefault(x => x.PlantId == id);

            if (item == null)
                return StatusCode(404);

            if (item.Count > 1)
                item.Count--;
            else
                cookieItems.Remove(item);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));

            BasketViewModel bv = new BasketViewModel();
            foreach (var ci in cookieItems)
            {
                BasketProductViewModel bi = new BasketProductViewModel
                {
                    Count = ci.Count,
                    Plant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == ci.PlantId)
                };
                bv.BasketProducts.Add(bi);
                bv.TotalPrice += (bi.Plant.SalePrice * bi.Count);
            }

            return PartialView("_BasketPartialView", bv);
        }
    }
}
