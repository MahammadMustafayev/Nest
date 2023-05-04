using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.Models;
using NestTest.Utilities.Extension;
using NestTest.ViewModels;
using System.Reflection.Metadata;

namespace NestTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private  NestDbContext _context { get;  }
        private IWebHostEnvironment _env { get; }
        public ProductController(NestDbContext context,IWebHostEnvironment env)
        {
            _context= context;
            _env= env;
        }
        public ActionResult Index(int? page)
        {
            ViewBag.Page = page;
            List<Product> prdcts =  _context.Products.Include(p => p.ProductImages).Include(p => p.Category).ToList();
            List<ProductVM> productVMs = new List<ProductVM>();
            foreach (var item in prdcts)
            {
                ProductVM product = new ProductVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = item.Category.Name,
                    Price = item.Price,
                    Image = item.ProductImages.FirstOrDefault(pi => pi.IsFront == true).Image,
                    IsDeleted = item.IsDeleted
                };
                productVMs.Add(product);
            }
            return View(productVMs);
        }
       
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Where(c=>c.isDeleted==false).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            ViewBag.Categories = _context.Categories.Where(c => c.isDeleted == false).ToList();
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            if (_context.Products.Any(p => p.Name.Trim().ToLower() == product.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "This name already exist");
                return View();
            }
            if (product.DiscountPrice == null)
            {
                product.DiscountPrice = product.Price;
            }
            else
            {
                if (product.Price < product.DiscountPrice)
                {
                    ModelState.AddModelError("DiscountPrice", "Malın endirimli qiyməti satış qiymətindən çox ola bilməz");
                }
            }
            product.ProductImages = new List<ProductImage>();
            if (product.Photos != null)
            {
                foreach (var file in product.Photos)
                {
                    if (IsPhotoOk(file) != "")
                    {
                        ModelState.AddModelError("Photos", IsPhotoOk(file));
                    }
                }
                foreach (var file in product.Photos)
                {
                    ProductImage image = new ProductImage
                    {
                        Image =  file.SaveFile(Path.Combine(_env.WebRootPath,"assets","imgs", "shop")),
                        IsFront = false,
                        IsBack = false,
                        Product = product
                    };
                    product.ProductImages.Add(image);
                }
            }
            if (product.PhotoBack != null)
            {
                if (IsPhotoOk(product.PhotoBack) != "")
                {
                    ModelState.AddModelError("PhotoBack", IsPhotoOk(product.PhotoBack));
                }
                product.ProductImages.Add(new ProductImage
                {
                    Image =  product.PhotoBack.SaveFile(Path.Combine(_env.WebRootPath,"assets","imgs", "shop")),
                    IsFront = false,
                    IsBack = true,
                    Product = product
                });
            }
            if (IsPhotoOk(product.PhotoFront) != "")
            {
                ModelState.AddModelError("PhotoFront", IsPhotoOk(product.PhotoFront));
            }
            product.ProductImages.Add(new ProductImage
            {
                Image =  product.PhotoFront.SaveFile(Path.Combine(_env.WebRootPath,"assets","imgs", "shop")),
                IsFront = true,
                IsBack = false,
                Product = product
            });



            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product is null) return RedirectToAction("Index", "Error");
            if (product.IsDeleted==true)
            {
                product.IsDeleted = false;
            }
            else
            {
                product.IsDeleted = true;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult PermaDelete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null) return RedirectToAction("Index", "Error");
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        private string IsPhotoOk(IFormFile file)
        {
            if (file.Checksize(500))
            {
                return $"{file.FileName} must be less than 500kb";
            }
            if (!file.CheckType("image/"))
            {
                return $"{file.FileName} is not image";
            }
            return "";
        }
    }
}
