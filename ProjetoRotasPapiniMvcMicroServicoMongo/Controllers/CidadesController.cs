using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Controllers
{
    [Authorize]
    public class CidadesController : Controller
    {
        // GET: Cidades
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

            return View(await Servico.VerificaCidade.EncontraTodasCidades());
        }

        // GET: Cidades/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cidade cidade = await Servico.VerificaCidade.EncontraCidadeUnica(id);

            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        // GET: Cidades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NomeCidade,Estado")] Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                Servico.VerificaCidade.GerarCidade(cidade);
                return RedirectToAction(nameof(Index));
            }
            return View(cidade);
        }

        // GET: Cidades/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cidade cidade = await Servico.VerificaCidade.EncontraCidadeUnica(id);
            
            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        // POST: Cidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NomeCidade,Estado")] Cidade cidade)
        {
            if (id != cidade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Servico.VerificaCidade.AtualizarCidade(id, cidade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CidadeExists(cidade.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cidade);
        }

        // GET: Cidades/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cidade cidade = await Servico.VerificaCidade.EncontraCidadeUnica(id);

            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        // POST: Cidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Cidade cidade = await Servico.VerificaCidade.EncontraCidadeUnica(id);

            if (cidade == null)
            {
                return NotFound();
            }

            Servico.VerificaCidade.RemoverCidade(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<Cidade> CidadeExists(string id)
        {
            return await Servico.VerificaCidade.EncontraCidadeUnica(id);
        }
    }
}
