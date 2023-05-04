using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.ViewModels;

namespace NestTest.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private NestDbContext  _context { get;  }
        public FooterViewComponent(NestDbContext context)
        {
            _context= context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            SettingVM setting = new SettingVM();
            setting.KeyValuePairs= await _context.Settings.ToDictionaryAsync(p=>p.Key,p=>p.Value);
            return View(await Task.FromResult(setting));
        }
    }
}
