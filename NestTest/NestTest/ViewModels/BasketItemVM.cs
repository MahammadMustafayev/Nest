namespace NestTest.ViewModels
{
    public class BasketItemVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Raiting { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
    }
}
