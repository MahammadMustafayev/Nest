using NestTest.Models;

namespace NestTest.ViewModels
{
    public class ShopVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int ProductCount { get; set; }
    }
}
