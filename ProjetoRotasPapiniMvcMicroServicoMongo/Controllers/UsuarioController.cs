﻿using System.Collections.Generic;
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
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var usuarioBuscado = await Servico.VerificaUsuario.EncontraNomeUsuario(usuario.NomeUsuario);

            if (ModelState.IsValid)
            {
                Servico.VerificaUsuario.GerarUsuario(usuario);

                return RedirectToAction(nameof(Index));
            }
            if(usuarioBuscado != null && usuario.Senha == usuarioBuscado.Senha)
            {
                List<Claim> userClaims = new()
                {
                    new Claim(ClaimTypes.Name, usuarioBuscado.NomeUsuario),
                    new Claim("Role", usuarioBuscado.Role),
                    new Claim(ClaimTypes.Role, usuarioBuscado.Role),
                };

                var myIdentity = new ClaimsIdentity(userClaims, "Usuario");
                var userPrincipal = new ClaimsPrincipal(new[] { myIdentity });

                await HttpContext.SignInAsync(userPrincipal);

                TempData["success"] = "Usuário logado!";

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
