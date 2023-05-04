using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NestTest.DAL;
using NestTest.Models;
using NestTest.Utilities.Extension;

namespace NestTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private NestDbContext _context { get;  }
        private IWebHostEnvironment _env { get; }
        public SliderController(NestDbContext context,IWebHostEnvironment env)
        {
            _context= context;  
            _env= env;
        }
       
        public IActionResult Index()
        {
            List<Slider> sliderList = _context.Sliders.ToList();
            return View(sliderList);
        }

        
        // GET: SliderController/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (slider != null)
            {
                if (slider.Photo.Checksize(200))
                {
                    ModelState.AddModelError("Photo", "This image must be better than 230 kb");
                    return View();
                }
                if (!slider.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError("Photo", "This image type must be image ");
                    return View();
                }
                slider.Image = slider.Photo.SaveFile(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider"));
                _context.Sliders.Add(slider);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Error");
           
        }
        public IActionResult Edit(int id)
        {
            if(id == null) return RedirectToAction("Index", "Error");
            Slider slider = _context.Sliders.Find(id);
            if(slider is null) return RedirectToAction("Index", "Error");
            return View(slider);
        }
        [HttpPost]
        public IActionResult Edit(int? id,Slider slider)
        {
            if(id != slider.Id || id is null) return RedirectToAction("Index", "Error");
            Slider existslider=_context.Sliders.Find(id);
            if (existslider is null) return RedirectToAction("Index", "Error");
            if (slider.Photo != null)
            {
                IFormFile file = slider.Photo;
                //if (file.CheckType("image/"))
                //{
                //    ModelState.AddModelError("Photo", "This file is must be image");
                //    return View();
                //}
                //if (file.Checksize(500))
                //{
                //    ModelState.AddModelError("Photo", "This file must beater 500 kb ");
                //    return View();
                //}
                string newFileName=Guid.NewGuid().ToString();
                newFileName += file.CutFileName(60);
                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", existslider.Image)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", existslider.Image));
                }
                file.SaveFileTest(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider",newFileName));
                existslider.Image= newFileName;
            }
            existslider.Description = slider.Description;
            existslider.Title = slider.Title;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Edit(int id)
        //{
        //    Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
        //    if (slider is null) return RedirectToAction("Index", "Error");
        //    return View(slider);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Slider slider)
        //{
        //    Slider existslider=_context.Sliders.FirstOrDefault(s=>s.Id== slider.Id);
        //    if (existslider is null) return RedirectToAction("Index","Error");
        //    string newFileName = null;
        //    if (existslider.Photo != null)
        //    {
        //        if (slider.Photo.CheckType("image/"))
        //        {
        //            ModelState.AddModelError("Photo", "This image type must be image ");
        //            return View(slider);
        //        }
        //        if (slider.Photo.Checksize(200))
        //        {
        //            ModelState.AddModelError("Photo", "This image must be better than 230 kb");
        //            return View(slider);
        //        }
        //        string fileName = slider.Photo.FileName;
        //        if (fileName.Length>64)
        //        {
        //            fileName = fileName.Substring(fileName.Length - 64, 64);
        //        }
        //        newFileName = Guid.NewGuid().ToString() + fileName;
        //        string path = Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", newFileName);
        //        using (FileStream fs = new FileStream(path, FileMode.Create))
        //        {
        //            slider.Photo.CopyTo(fs);
        //        }
        //    }
        //    if (newFileName != null)
        //    {
        //        string deletePath = Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", existslider.Image);
        //        if (System.IO.File.Exists(deletePath))
        //        {
        //            System.IO.File.Delete(deletePath);
        //        }
        //        existslider.Image = newFileName;
        //    }
        //    existslider.Title = slider.Title;
        //    existslider.Description = slider.Description;
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //public static void RemoveFile(string path)
        //{
        //    path = Path.Combine(_env.WebRootPathpath);

        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }
        //}
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return RedirectToAction("Index", "Error");
            slider.IsDeleted=true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult PermaDelete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return RedirectToAction("Index", "Error");
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath,"assets","imgs","slider",slider.Image)))
            {
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", slider.Image));
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        

    }
}
