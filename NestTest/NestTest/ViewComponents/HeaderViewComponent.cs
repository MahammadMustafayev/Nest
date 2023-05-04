using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.ViewModels;
using Newtonsoft.Json;

namespace NestTest.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private NestDbContext _context { get;  }

        private readonly IHttpContextAccessor _accessor;

        public HeaderViewComponent(NestDbContext context,IHttpContextAccessor accessor)
        {
            _context= context;
            _accessor = accessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            SettingVM vM = new SettingVM();
            vM.KeyValuePairs= await _context.Settings.ToDictionaryAsync(p=>p.Key,p=>p.Value);
            if (_accessor.HttpContext.Request.Cookies["Basket"] != null)
            {
                List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["Basket"]);
                vM.Count=basketVMs.Sum(b => b.Count);
            }
            
            return View(vM);
        }
    }
}
