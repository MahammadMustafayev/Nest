namespace NestTest.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
