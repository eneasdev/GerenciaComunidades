using Microsoft.AspNetCore.Mvc;
using Novo.Infra;

namespace Novo.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(GeComuContext context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}