using NestTest.DAL;
using NestTest.Models;

namespace NestTest.Services
{
    public class _LayoutServices
    {
        private NestDbContext  _context { get;  }
        public _LayoutServices(NestDbContext context)
        {
            _context= context;
        }
        public Dictionary<string, string> GetSettings() 
        {
            return _context.Settings.ToDictionary(s=>s.Key, s=>s.Value);
        }
    }
}
