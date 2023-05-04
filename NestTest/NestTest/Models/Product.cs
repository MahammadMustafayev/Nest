

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NestTest.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100),Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        //[MinLength(0)]
        public double Price { get; set; }
        public double CostPrice { get; set; }
        public double? DiscountPrice { get; set; }
        //[MaxLength(5),MinLength(0)]
        public int Raiting { get; set; }
        [Required]
        public int StockCount { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }
        [NotMapped]
        public IFormFile PhotoFront { get; set; }
        [NotMapped]
        public IFormFile PhotoBack { get; set; }
        [NotMapped]
        public List<int> PhotoIds { get; set; }
    }
}
