using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NestTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ErrorController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

       
    }
}
