using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;
using ProjetoRotasPapiniMvcMicroServicoMongo.Servico;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Controllers
{
    public class RotaReadFilePathController : Controller
    {
        IWebHostEnvironment _appEnvironment;

        public RotaReadFilePathController(IWebHostEnvironment environment)
        {
            _appEnvironment = environment;
        }
        public IActionResult Index()
        {
            ViewBag.Cabecalho = ReadFilePath.LerCabecalho(_appEnvironment.WebRootPath);
            return View();
        }

        public IActionResult EscolhaServico()
        {
            var listaCabecalho = Request.Form["Column"].ToList();
            List<string> servicos = new();

            foreach (var cabecalho in listaCabecalho)
                if (cabecalho == "SERVIÇO" || cabecalho == "SERVICO")
                    servicos = ReadFilePath.LerColuna(cabecalho, _appEnvironment.WebRootPath);

            ViewBag.Servico = servicos.Distinct().ToList();
            ViewBag.CabecalhoEscolhido = listaCabecalho;
            return View();
        }

        public IActionResult EscolhaCidade(string servico, List<string> listaCabecalhos)
        {
            List<string> listaCidades = new();

            foreach (var cabecalho in listaCabecalhos)
                if (cabecalho == "CIDADE")
                    listaCidades = ReadFilePath.LerColuna(cabecalho, _appEnvironment.WebRootPath);

            ViewBag.Cidade = listaCidades.Distinct().ToList();
            ViewBag.Servico = servico;
            return View();
        }

        public async Task<IActionResult> EscolhaTime(string cidade)
        {
            var listaTimes = await VerificaTime.EncontraTodosTimes();
            List<Time> timeCidade = new();

            foreach (var time in listaTimes)
                if (time.Cidade.NomeCidade == cidade)
                    timeCidade.Add(time);

            ViewBag.Time = timeCidade;
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

    }
}
