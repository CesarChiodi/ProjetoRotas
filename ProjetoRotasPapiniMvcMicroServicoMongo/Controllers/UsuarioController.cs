using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoRotasPapiniMvcMicroServicoMongo.Data;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Controllers
{
    
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public IActionResult Index()
        {
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

        //// GET: Pessoas/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Usuario usuario = await Servico.VerificaUsuario.EncontraUsuarioUnico(id);
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(usuario);
        //}

        //// POST: Pessoas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    Usuario usuario = await Servico.VerificaUsuario.EncontraUsuarioUnico(id);

        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    Servico.VerificaUsuario.RemoverUsuario(id);

        //    return RedirectToAction(nameof(Index));
        //}

        //private async Task<Usuario> UsuarioExists(string id)
        //{
        //    return await Servico.VerificaUsuario.EncontraUsuarioUnico(id);
        //}






























        //private UserManager<ApplicationUser> _userManager;
        //public UsuarioController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Usuario usuario)
        //{
        //    ApplicationUser usuarioLog = new ApplicationUser
        //    {
        //        UserName = usuario.NomeUsuario,
        //        Email = usuario.Email
        //    };

        //    IdentityResult result = await _userManager.CreateAsync(usuarioLog, usuario.Senha);

        //    if (result.Succeeded)
        //    {
        //        ViewBag.Message = "Usuário Cadastrado";
        //    }
        //    else
        //    {
        //        foreach (IdentityError error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(usuario);
        //}

    }
}
