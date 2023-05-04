namespace NestTest.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool isDeleted { get; set; }
        public List<Product> Products { get; set; }
    }
}
