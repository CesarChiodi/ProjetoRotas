using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Controllers
{

    public class UsuarioController : Controller
    {
        // GET: Usuario
        public IActionResult Index()
        {
            string usuario = "Anonimo";
            bool autenticacao = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                usuario = HttpContext.User.Identity.Name;
                autenticacao = true;

                if (HttpContext.User.IsInRole("admin"))
                {
                    ViewBag.Role = "admin";
                }
                else
                {
                    ViewBag.Role = "usuario";
                }
            }
            else
            {
                usuario = "Não Logado";
                autenticacao = false;
                ViewBag.Role = "";
            }

            ViewBag.Usuario = usuario;
            ViewBag.Authenticate = autenticacao;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            Usuario usuarioBuscado = await Servico.VerificaUsuario.EncontraNomeUsuario(usuario.NomeUsuario);

            if (ModelState.IsValid)
            {
                Servico.VerificaUsuario.GerarUsuario(usuario);

                return RedirectToAction(nameof(Index));
            }
            if (usuarioBuscado != null && usuario.Senha == usuarioBuscado.Senha)
            {
                List<Claim> listaUsuariosClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuarioBuscado.NomeUsuario),
                    new Claim("Role", usuarioBuscado.Role),
                    new Claim(ClaimTypes.Role, usuarioBuscado.Role),
                };

                ClaimsIdentity identity = new ClaimsIdentity(listaUsuariosClaims, "Usuario");
                ClaimsPrincipal usuarioPrincipal = new ClaimsPrincipal(new[] { identity });

                await HttpContext.SignInAsync(usuarioPrincipal);


                return RedirectToRoute(new { controller = "RotaReadFilePath", action = "Index" });
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            Servico.VerificaUsuario.GerarUsuario(usuario);
            return RedirectToAction(nameof(Index));
        }



    }
}
