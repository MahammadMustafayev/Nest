using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.Models;

namespace NestTest.Areas.Manage.Controllers
{
      [Area("Manage")]
    public class CategoryController : Controller
    {
        private NestDbContext _context { get;  }
        public CategoryController(NestDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> category = _context.Categories.Include(c => c.Products).ToList();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (_context.Categories.FirstOrDefault(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim()) != null) return RedirectToAction("Index");
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(c=>c.Id==id);
            if (category == null) return RedirectToAction("Index", "Error");
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            Category existcategory = _context.Categories.FirstOrDefault(c=>c.Id == category.Id);
            if (existcategory == null) return RedirectToAction("Index", "Error");
            existcategory.Name = category.Name;
            existcategory.Logo= category.Logo;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult PermaDelete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            if (category.isDeleted==true)
            {
                category.isDeleted = false;
            }
            else
            {
                category.isDeleted = true;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
