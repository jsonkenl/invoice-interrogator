using Microsoft.AspNetCore.Mvc;

namespace InvoiceInterrogator.Net.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
