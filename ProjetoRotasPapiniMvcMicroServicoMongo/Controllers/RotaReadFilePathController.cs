using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;
using ProjetoRotasPapiniMvcMicroServicoMongo.Servico;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Controllers
{
    [Authorize]
    public class RotaReadFilePathController : Controller
    {
        IWebHostEnvironment _appEnvironment;

        public RotaReadFilePathController(IWebHostEnvironment environment)
        {
            _appEnvironment = environment;
        }
        public IActionResult Index()
        {
            string user = "Anonymous";
            bool authenticate = false;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = HttpContext.User.Identity.Name;
                authenticate = true;

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
                user = "Não Logado";
                authenticate = false;
                ViewBag.Role = "";
            }

            ViewBag.Usuario = user;
            ViewBag.Authenticate = authenticate;
            ViewBag.Cabecalho = ReadFilePath.LerCabecalho(_appEnvironment.WebRootPath);
            return View();
        }

        public IActionResult EscolhaServico()
        {
            var listaCabecalho = Request.Form["Column"].ToList();
            List<string> servicos = new();

            foreach (var cabecalho in listaCabecalho)
            {
                if (cabecalho == "SERVIÇO" || cabecalho == "SERVICO")
                {
                    servicos = ReadFilePath.LerColuna(cabecalho, _appEnvironment.WebRootPath);
                }
            }

            ViewBag.Servico = servicos.Distinct().ToList();
            ViewBag.Cabecalho = listaCabecalho;
            return View();
        }

        public IActionResult EscolhaCidade(string servico)
        {
            List<string> listaCidades = new();
            List<string> cabecalhoEscolhido = Request.Form["cabecalho"].ToList();

            foreach (var cabecalho in cabecalhoEscolhido)
            {
                if (cabecalho == "CIDADE")
                {
                    listaCidades = ReadFilePath.LerColuna(cabecalho, _appEnvironment.WebRootPath);
                }
            }

            ViewBag.Cidade = listaCidades.Distinct().ToList();
            ViewBag.Servico = servico;
            ViewBag.Cabecalho = cabecalhoEscolhido;

            return View();
        }

        public async Task<IActionResult> EscolhaTime(string cidade)
        {
            var cabecalhoEscolhido = Request.Form["cabecalho"].ToList();
            var servicoEscolhido = Request.Form["servico"].ToString();
            cidade = Request.Form["cidade"].ToString();
            var listaTimes = await VerificaTime.EncontraTodosTimes();
            List<Time> timeCidade = new List<Time>();
            var servico = servicoEscolhido.Replace(",", "");

            foreach (var time in listaTimes)
            {
                if (time.Cidade.NomeCidade == cidade)
                {
                    timeCidade.Add(time);
                }
            }

            ViewBag.Cabecalho = cabecalhoEscolhido;
            ViewBag.Servico = servico;
            ViewBag.Time = timeCidade;
            ViewBag.Cidade = cidade;

            return View();
        }


        //public IActionResult EscreverWord()
        //{
        //    List<string> cabecalho = Request.Form["Coluna"].ToList();

        //    List<Rotas> arquivoRotas = new ReadFileXLS()
        //        .LerArquivoXLS(cabecalho.Select(int.Parse).ToList());

        //    new ProcessFileXLS().EscreverDocx(arquivoRotas);

        //    return RedirectToAction("ListaServicos");
        //}


        public IActionResult GerarEscritaWord()
        {
            var cabecalhoEscolhido = Request.Form["cabecalho"].ToList();
            var servicoEscolhido = Request.Form["servico"].ToString();
            var cidadeEscolhida = Request.Form["cidade"].ToString();
            var listaTimes = Request.Form["time"].ToList();

            var servico = servicoEscolhido.Replace(",", "");

            var cidade = cidadeEscolhida.Replace(",", "");

            new EscreverFilePath().EscreverWord(cabecalhoEscolhido, listaTimes, servico, cidade, _appEnvironment.WebRootPath);
            return RedirectToAction(nameof(Index));
        }

        public FileResult Download()
        {
            var folder = _appEnvironment.WebRootPath + "\\file\\";
            var pathFinal = folder + "RotasGeradas.docx";
            byte[] bytes = System.IO.File.ReadAllBytes(pathFinal);
            string contentType = "application/octet-stream";
            return File(bytes, contentType, "RotasGeradas.docx");
        }
    }
}
