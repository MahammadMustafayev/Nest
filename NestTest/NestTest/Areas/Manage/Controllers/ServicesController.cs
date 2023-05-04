using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NestTest.DAL;
using NestTest.Models;

namespace NestTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ServicesController : Controller
    {
        private NestDbContext   _context { get;  }
        public ServicesController(NestDbContext context)
        {
            _context = context;
        }

        // GET: ServicesController
        public ActionResult Index()
        {
            List<Setting> setting = _context.Settings.ToList();
            return View(setting);
        }

        
        // GET: ServicesController/Edit/5
        public ActionResult Edit(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting == null) return RedirectToAction("Index", "Error");
            return View(setting);
        }

        // POST: ServicesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Setting setting)
        {
            Setting exsistsetting= _context.Settings.FirstOrDefault(s=>s.Id == setting.Id);
            if (exsistsetting is null) return RedirectToAction("Index", "Error");
            exsistsetting.Key= setting.Key;
            exsistsetting.Value= setting.Value;
            _context.SaveChanges();
            return RedirectToAction("Index", "Services");
        }

        // GET: ServicesController/Delete/5
      
        
        public ActionResult Delete(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return RedirectToAction("Index", "Error");
            if (setting.IsDeleted)
            {
                setting.IsDeleted = false;
            }
            else
            {
                setting.IsDeleted = true;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: ServicesController/Delete/5
     
        public ActionResult PermaDelete(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(s=>s.Id==id);
            if (setting is null) return RedirectToAction("Index", "Error");
            _context.Settings.Remove(setting);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }
    }
}
