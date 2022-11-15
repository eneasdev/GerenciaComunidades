using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novo.Infra;

namespace Novo.Controllers
{
    [AllowAnonymous]
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