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
    public class TimesController : Controller
    {
        // GET: Times
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
            return View(await Servico.VerificaTime.EncontraTodosTimes());
        }

        // GET: Times/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;
            if (HttpContext.User.IsInRole("admin"))
                ViewBag.Role = "admin";
            else
                ViewBag.Role = "usuario";

            if (id == null)
            {
                return NotFound();
            }

            Time time = await Servico.VerificaTime.EncontraTimeUnico(id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // GET: Times/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;
            if (HttpContext.User.IsInRole("admin"))
                ViewBag.Role = "admin";
            else
                ViewBag.Role = "usuario";

            List<Pessoa> pessoasCadastradas = await Servico.VerificaPessoa.EncontraTodasPessoa();
            List<Time> listaTimes = await Servico.VerificaTime.EncontraTodosTimes();

            List<Pessoa> comTime = new List<Pessoa>();

            foreach (Time time in listaTimes)
            {
                foreach (Pessoa pessoaTime in time.PessoaTime)
                {
                    comTime.Add(pessoaTime);
                }
            }

            List<Pessoa> semTime = new List<Pessoa>();

            foreach (Pessoa registeredPerson in pessoasCadastradas)
            {
                if (comTime.Find(time => time.Id.Equals(registeredPerson.Id)) == null)
                {
                    semTime.Add(registeredPerson);
                }
            }

            ViewBag.Pessoa = semTime;
            ViewBag.Cidade = await Servico.VerificaCidade.EncontraTodasCidades();

            return View();
        }

        // POST: Times/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeTime")] Time time)
        {


            string cidade = Request.Form["Cidade"].ToString();
            time.Cidade = await Servico.VerificaCidade.EncontraCidadeUnica(cidade);

            List<string> listaPessoasTime = Request.Form["Pessoa"].ToList();
            time.PessoaTime = new List<Pessoa>();

            foreach (string pessoaTime in listaPessoasTime)
            {
                Pessoa pessoa = await Servico.VerificaPessoa.EncontraPessoaUnica(pessoaTime);
                time.PessoaTime.Add(pessoa);
            }

            if (ModelState.IsValid)
            {
                Servico.VerificaTime.GerarTime(time);

                return RedirectToAction(nameof(Index));
            }

            return View(time);
        }

        // GET: Times/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;
            if (HttpContext.User.IsInRole("admin"))
                ViewBag.Role = "admin";
            else
                ViewBag.Role = "usuario";

            if (id == null)
            {
                return NotFound();
            }

            Time time = await Servico.VerificaTime.EncontraTimeUnico(id);

            if (time == null)
            {
                return NotFound();
            }

            List<Pessoa> listaPessoasCadastradas = await Servico.VerificaPessoa.EncontraTodasPessoa();
            List<Time> listaTimes = await Servico.VerificaTime.EncontraTodosTimes();

            List<Pessoa> comTime = new List<Pessoa>();

            foreach (Time timeModificacao in listaTimes)
            {
                foreach (Pessoa pessoaTime in timeModificacao.PessoaTime)
                {
                    comTime.Add(pessoaTime);
                }
            }

            List<Pessoa> semTime = new List<Pessoa>();

            foreach (Pessoa pessoaCadastrada in listaPessoasCadastradas)
            {
                if (comTime.Find(item => item.Id.Equals(pessoaCadastrada.Id)) == null)
                {
                    semTime.Add(pessoaCadastrada);
                }
            }

            ViewBag.PessoaTime = time.PessoaTime;
            ViewBag.Pessoa = semTime;
            ViewBag.Cidade = await Servico.VerificaCidade.EncontraTodasCidades();

            return View(time);
        }

        // POST: Times/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomeTime")] Time time)
        {
            if (id != time.Id)
            {
                return NotFound();
            }

            string nomeTime = time.NomeTime;
            time = await Servico.VerificaTime.EncontraTimeUnico(id);
            time.NomeTime = nomeTime;

            string cidade = Request.Form["Cidade"].ToString();
            time.Cidade = await Servico.VerificaCidade.EncontraCidadeUnica(cidade); 

            List<string> pessoa = Request.Form["Pessoa"].ToList();
            List<string> listaPessoasTime = Request.Form["PessoaTime"].ToList();

            if (time.PessoaTime.Count > listaPessoasTime.Count)
            {
                if (listaPessoasTime != null || listaPessoasTime.Any())
                {
                    List<Pessoa> listaPessoa = new List<Pessoa>();

                    foreach (string objetoPessoaTime in listaPessoasTime)
                    {
                        Pessoa pessoaTime = await Servico.VerificaPessoa.EncontraPessoaUnica(objetoPessoaTime);
                        time.PessoaTime.Remove(time.PessoaTime.Find(memberToRemove => memberToRemove.Id == objetoPessoaTime));
                        pessoaTime.Ativo = false;
                        Servico.VerificaPessoa.AtualizarPessoa(objetoPessoaTime, pessoaTime);
                    }
                }
            }

            foreach (string pessoaSemTime in pessoa)
            {
                time.PessoaTime.Add(await Servico.VerificaPessoa.EncontraPessoaUnica(pessoaSemTime));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Servico.VerificaTime.AtualizarTime(id, time);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (TimeExists(time.Id) == null)
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
            return View(time);
        }

        // GET: Times/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Usuario = HttpContext.User.Identity.Name;
            if (HttpContext.User.IsInRole("admin"))
                ViewBag.Role = "admin";
            else
                ViewBag.Role = "usuario";

            if (id == null)
            {
                return NotFound();
            }

            Time time = await Servico.VerificaTime.EncontraTimeUnico(id);

            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Time time = await Servico.VerificaTime.EncontraTimeUnico(id);

            if (time == null)
            {
                return NotFound();
            }

            Servico.VerificaTime.RemoverTime(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<Time> TimeExists(string id)
        {
            return await Servico.VerificaTime.EncontraTimeUnico(id);
        }
    }
}
