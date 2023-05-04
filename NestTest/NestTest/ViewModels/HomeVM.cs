using NestTest.Models;

namespace NestTest.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> RecentlyProduct { get; set; }
        public List<Product> RatedProduct { get; set; }
    }
}
