//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using Novo.Infra;
//using Novo.Models.Domain;
//using Novo.Models.Enums;
//using System.Security.Claims;

//namespace Novo.Controllers
//{
//    public class LoginController : Controller
//    {
//        private GeComuContext _context;

//        public LoginController(GeComuContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public ActionResult UsuarioLogin()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult UsuarioLogin(Usuario usuario)
//        {
//            var user = _context.Usuarios.FirstOrDefault(u => u.Login == usuario.Login && u.Senha == usuario.Senha);

//            if (user != null)
//            {
//                DefinirPermissao(user);

//                return RedirectToAction("Index", "Home");
//            }

//            ViewBag.Message = "Credenciais inválidas...";

//            return View(usuario);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync();
//            return RedirectToAction("Index", "Home");
//        }

//        private RedirectToActionResult DefinirPermissao (Usuario user)
//        {
//            var userClaims = new List<Claim>()
//                {
//                    //define o cookie
//                    new Claim(ClaimTypes.Name, user.Login),
//                };
//            var minhaIdentity = new ClaimsIdentity(userClaims, user.NivelAcesso.ToString());
//            var userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });

//            //cria o cookie
//            HttpContext.SignInAsync(userPrincipal);

//            return RedirectToAction("Index", "Home");
//        }
//    }
//}