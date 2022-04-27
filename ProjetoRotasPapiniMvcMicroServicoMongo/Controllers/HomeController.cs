using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            string usuarioId = "Anonimo";
            bool autenticacao = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                usuarioId = HttpContext.User.Identity.Name;
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
                usuarioId = "Não Logado";
                autenticacao = false;
                ViewBag.Role = "";
            }

            ViewBag.Usuario = usuarioId;
            ViewBag.Authenticate = autenticacao;
            List<Usuario> listaUsuario = await Servico.VerificaUsuario.EncontraTodosUsuarios();
            if (listaUsuario.Count < 1)
            {
                Servico.VerificaUsuario.GerarUsuario(new Usuario { NomeUsuario = "admin", Senha = "admin", Role = "admin" });
                ViewBag.Usuario = "admin";
                ViewBag.Role = "admin";
                ViewBag.Authenticate = true;

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ReceberArquivo()
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
        public async Task<IActionResult> ReceberArquivo([Bind("Id,FileName, File")] Arquivo arquivo)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo.File.FileName);
                string estensao = Path.GetExtension(arquivo.File.FileName);
                arquivo.FileName = nomeArquivo = nomeArquivo + estensao;
                string path = Path.Combine(wwwRootPath + "/file", nomeArquivo);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await arquivo.File.CopyToAsync(fileStream);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(arquivo);
        }
    }
}
