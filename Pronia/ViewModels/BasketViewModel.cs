using Pronia.ViewModels;

namespace Pronia.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketProductViewModel> BasketProducts { get; set; } = new List<BasketProductViewModel>();
        public decimal TotalPrice { get; set; }
    }
}
