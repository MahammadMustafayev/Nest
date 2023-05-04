namespace NestTest.ViewModels
{
    public class PaginateVM<T>
    {
        public List<T> Items { get; set; }
        public int ActivePage { get; set; }
        public int PageCount { get; set; }
    }
}
