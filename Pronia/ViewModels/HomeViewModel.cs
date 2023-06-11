using Pronia.Models;

namespace Pronia.ViewModels
{
    public class HomeViewModel
    {
        public List<Plant> FeaturedPlant { get; set; }
        public List<Plant> BestSellerPlant { get; set; }
        public List<Plant> LatestPlant { get; set; }

    }
}
