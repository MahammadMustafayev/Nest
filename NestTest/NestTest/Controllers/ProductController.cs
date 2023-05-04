using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.Models;
using NestTest.ViewModels;
using Newtonsoft.Json;

namespace NestTest.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        private NestDbContext _context { get; }
        public ProductController(NestDbContext context)
        {
            _context= context;
        }
        public ActionResult Index(int? page)
        {
            Response.Cookies.Append("Surname", "Aydinli");
            HttpContext.Session.SetString("Name", "Tural");
            ViewBag.Page = page;
            ViewBag.ProductCount = _context.Products.Where(p => p.IsDeleted == false).Count();
            ViewBag.Categories = _context.Categories.Where(p => p.isDeleted == false).Include(c => c.Products);
            return View(_context.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.Id).Take(10).Include(p => p.Category).Include(pi => pi.ProductImages));
        }
        public IActionResult LoadMore(int skip)
        {

            return PartialView("_ProductPartial", _context.Products.Where(p => p.IsDeleted == false)
                                                    .OrderByDescending(p => p.Id)
                                                    .Skip(skip)
                                                    .Take(10)
                                                    .Include(pi => pi.ProductImages)
                                                    .Include(c => c.Category));
        }
        public IActionResult Basket()
        {
            List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["Basket"]);
            return Json(basket);
        }
        private List<BasketVM> GetBasket()
        {
            List<BasketVM> basketItems = new List<BasketVM>();
            if (Request.Cookies["Basket"] != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["Basket"]);
            }
            return basketItems;
        }
        private void UpdateBasket(int id)
        {
            List<BasketVM> basketItem= GetBasket();
            BasketVM basketVM = basketItem.Find(bi=>bi.ProductId==id);
            if (basketVM != null)
            {
                basketVM.Count += 1;
            }
            else
            {
                basketVM = new BasketVM
                {
                    ProductId= id,
                    Count = 1,
                };
                basketItem.Add(basketVM);
            }
            Response.Cookies.Append("Basket",JsonConvert.SerializeObject(basketItem));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int id)
        {
            if (id == null) return RedirectToAction("Index", "Error");
            Product dbproduct = await _context.Products.FindAsync(id);
            if (dbproduct == null) return RedirectToAction("Index", "Error");
            UpdateBasket(id); 
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public IActionResult Cart()
        {
            List<BasketVM> basket= GetBasket();
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            foreach (var item in basket)
            {
                Product dbProduct = _context.Products.Where(p=>p.IsDeleted==false)
                                   .Include(pi=>pi.ProductImages)
                                   .FirstOrDefault(p=>p.Id==item.ProductId);
                if (dbProduct == null) return RedirectToAction("Index", "Error");
                BasketItemVM basketItem =  new BasketItemVM
                {
                    ProductId= dbProduct.Id,
                    Name=dbProduct.Name,
                    Image=dbProduct.ProductImages.FirstOrDefault(pi=>pi.IsFront==true).Image,
                    Raiting=dbProduct.Raiting,
                    Price=dbProduct.Price,
                    Count=item.Count,
                    IsActive = dbProduct.StockCount > 0 ? true : false,
                };
                basketItems.Add(basketItem);
            }
            return View(basketItems);
        }

        public IActionResult GetSession()
        {
            return Json(HttpContext.Session.GetString("Name")+" " + Request.Cookies["Surname"]);
        }
        public IActionResult CategoryFilter(int CategoryId)
        {
            if(_context.Categories.Find(CategoryId)==null) return RedirectToAction("Index", "Error");
            return PartialView(_context.Products.Where(p=>p.IsDeleted==false && p.CategoryId==CategoryId)
                               .OrderByDescending(p=>p.Id)
                               .Include(c=>c.Category)
                               .Include(pi=>pi.ProductImages));
        }

        
        

        
    }
}
