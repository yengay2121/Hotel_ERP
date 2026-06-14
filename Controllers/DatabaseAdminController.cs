using Microsoft.AspNetCore.Mvc;

namespace HotelERP.Controllers
{
    public class DatabaseAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
