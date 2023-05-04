using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.Models;
using NestTest.ViewModels;

namespace NestTest.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private NestDbContext _context;
        public ProductViewComponent(NestDbContext context)
        {
            _context=context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int page=1)
        {
            int pageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / 10);
            if (page<0 || page>pageCount)
            {
                page = 1;
            }
            List<Product> products = await _context.Products.Where(p=>p.IsDeleted==false)
                                     .OrderByDescending(p => p.Id)
                                     .Skip((page-1)*10)
                                     .Take(10)
                                     .Include(c=>c.Category)
                                     .Include(pi=>pi.ProductImages)
                                     .ToListAsync();
            PaginateVM<Product> paginate = new PaginateVM<Product>
            {
                Items=products,
                ActivePage=page,
                PageCount=pageCount
            };
            return View(await Task.FromResult(paginate));
        }
         
    }
}
