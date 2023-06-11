using Pronia.DAL;
using Pronia.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Pronia.Services
{
    public class LayoutService
    {
        private readonly ProniaDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LayoutService(ProniaDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }
        public BasketViewModel GetBasket()
        {
            var bv = new BasketViewModel();
            var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if (basketJson != null)
            {
                var cookieItems = JsonConvert.DeserializeObject<List<BasketProductCookieViewModel>>(basketJson);
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
            }
            return bv;
        }
    }
}
