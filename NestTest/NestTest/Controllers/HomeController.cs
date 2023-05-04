using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.Models;
using NestTest.ViewModels;
using System.Diagnostics;

namespace NestTest.Controllers
{
    
    public class HomeController : Controller
    {
        private  NestDbContext _context;

        public HomeController(NestDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IQueryable<Product> query = _context.Products.Where(p=>p.IsDeleted==false).Include(p => p.ProductImages).Include(p => p.Category).AsQueryable();
            HomeVM homeVM = new HomeVM
            {
                Sliders=_context.Sliders.Where(s=>s.IsDeleted==false).ToList(),
                Categories= _context.Categories.Where(c=>c.isDeleted==false).Include(c=>c.Products).ToList(),
                Products=query.Take(10).ToList(),
                RecentlyProduct= query.OrderByDescending(p=>p.Id).Take(3).ToList(),
                RatedProduct=query.OrderByDescending(p=>p.Raiting).Take(3).ToList()
            };
            return View(homeVM);
        }
        
        public IActionResult Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Error");
            Product product = _context.Products
                            .Include(p => p.ProductImages)
                            .Include(c => c.Category)
                            .FirstOrDefault(p => p.Id == id && p.IsDeleted==false );
            if (product is null) return RedirectToAction("Index", "Error");
            ViewBag.Categories = _context.Categories.Where(c => c.isDeleted == false).Include(c => c.Products).ToList();
            return View(product);
        }


    }
}