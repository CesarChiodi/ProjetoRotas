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
            string user = "Anonymous";
            bool authenticate = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.User.Identity.Name;
                authenticate = true;

                if (HttpContext.User.IsInRole("admin"))
                    ViewBag.Role = "admin";
                else
                    ViewBag.Role = "usuario";
            }
            else
            {
                user = "Não Logado";
                authenticate = false;
                ViewBag.Role = "";
            }

            ViewBag.Usuario = user;
            ViewBag.Authenticate = authenticate;
            var usuario = await Servico.VerificaUsuario.EncontraTodosUsuarios();
            if(usuario.Count < 1)
            {
                Servico.VerificaUsuario.GerarUsuario(new Usuario { NomeUsuario = "admin", Senha = "admin",  Role = "admin" });
                ViewBag.Usuario = "admin";
                ViewBag.Role="admin";
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
            string user = "Anonymous";
            bool authenticate = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.User.Identity.Name;
                authenticate = true;

                if (HttpContext.User.IsInRole("admin"))
                    ViewBag.Role = "admin";
                else
                    ViewBag.Role = "usuario";
            }
            else
            {
                user = "Não Logado";
                authenticate = false;
                ViewBag.Role = "";
            }

            ViewBag.Usuario = user;
            ViewBag.Authenticate = authenticate;
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
                string extension = Path.GetExtension(arquivo.File.FileName);
                arquivo.FileName = nomeArquivo = nomeArquivo + extension;
                string path = Path.Combine(wwwRootPath + "/file", nomeArquivo);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await arquivo.File.CopyToAsync(fileStream);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(arquivo);
        }
    }
}
