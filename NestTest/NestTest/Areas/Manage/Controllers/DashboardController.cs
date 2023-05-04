using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NestTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DashboardController : Controller
    {
        // GET: DashboardController
        public ActionResult Index()
        {
            return View();
        }

        

    }
}
