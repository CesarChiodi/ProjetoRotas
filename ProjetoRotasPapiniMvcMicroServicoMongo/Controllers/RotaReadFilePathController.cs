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
            ViewBag.Cabecalho = ReadFilePath.LerCabecalho(_appEnvironment.WebRootPath);
            return View();
        }

        public IActionResult EscolhaServico()
        {
            List<string> listaCabecalho = Request.Form["Column"].ToList();
            List<string> servicos = new List<string>();

            foreach (string cabecalho in listaCabecalho)
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
            List<string> listaCidades = new List<string>();
            List<string> cabecalhoEscolhido = Request.Form["cabecalho"].ToList();

            foreach (string cabecalho in cabecalhoEscolhido)
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
            List<string> cabecalhoEscolhido = Request.Form["cabecalho"].ToList();
            string servicoEscolhido = Request.Form["servico"].ToString();
            cidade = Request.Form["cidade"].ToString();

            List<Time> listaTimes = await VerificaTime.EncontraTodosTimes();
            List<Time> timeCidade = new List<Time>();

            string servico = servicoEscolhido.Replace(",", "");

            foreach (Time time in listaTimes)
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
            List<string> cabecalhoEscolhido = Request.Form["cabecalho"].ToList();
            string servicoEscolhido = Request.Form["servico"].ToString();
            string cidadeEscolhida = Request.Form["cidade"].ToString();
            List<string> listaTimes = Request.Form["time"].ToList();

            string servico = servicoEscolhido.Replace(",", "");
            string cidade = cidadeEscolhida.Replace(",", "");

            new EscreverFilePath().EscreverWord(cabecalhoEscolhido, listaTimes, servico, cidade, _appEnvironment.WebRootPath);
            return View();
        }

        public FileResult Download()
        {
            string pastaArquivo = _appEnvironment.WebRootPath + "\\file\\";
            string arquivoPronto = pastaArquivo + "RotasGeradas.docx";

            byte[] bytes = System.IO.File.ReadAllBytes(arquivoPronto);
            string contentType = "application/octet-stream";

            return File(bytes, contentType, "RotasGeradas.docx");
        }
    }
}
