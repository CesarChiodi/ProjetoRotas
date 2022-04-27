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
    public class PessoasController : Controller
    {
        // GET: Pessoas
        public async Task<IActionResult> Index()
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

            return View(await Servico.VerificaPessoa.EncontraTodasPessoa());
        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;

            if (HttpContext.User.IsInRole("admin"))
            {
                ViewBag.Role = "admin";
            }
            else
            {
                ViewBag.Role = "usuario";
            }

            if (id == null)
            {
                return NotFound();
            }

            Pessoa pessoa = await Servico.VerificaPessoa.EncontraPessoaUnica(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;

            if (HttpContext.User.IsInRole("admin"))
            {
                ViewBag.Role = "admin";
            }
            else
            {
                ViewBag.Role = "usuario";
            }

            return View();
        }

        // POST: Pessoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,NomePessoa")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                Servico.VerificaPessoa.GerarPessoa(pessoa);

                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;

            if (HttpContext.User.IsInRole("admin"))
            {
                ViewBag.Role = "admin";
            }
            else
            {
                ViewBag.Role = "usuario";
            }

            if (id == null)
            {
                return NotFound();
            }

            Pessoa pessoa = await Servico.VerificaPessoa.EncontraPessoaUnica(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomePessoa")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Servico.VerificaPessoa.AtualizarPessoa(id, pessoa);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (PessoaExists(pessoa.Id) == null)
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

            return View(pessoa);
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;
            if (HttpContext.User.IsInRole("admin"))
            {
                ViewBag.Role = "admin";
            }
            else
            {
                ViewBag.Role = "usuario";
            }

            if (id == null)
            {
                return NotFound();
            }

            Pessoa pessoa = await Servico.VerificaPessoa.EncontraPessoaUnica(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Pessoa pessoa = await Servico.VerificaPessoa.EncontraPessoaUnica(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            Servico.VerificaPessoa.RemoverPessoa(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<Pessoa> PessoaExists(string id)
        {
            return await Servico.VerificaPessoa.EncontraPessoaUnica(id);
        }
    }
}
