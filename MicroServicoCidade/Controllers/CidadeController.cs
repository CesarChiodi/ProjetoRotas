using System.Collections.Generic;
using MicroServicoCidade.Model;
using MicroServicoCidade.Servico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoCidade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadeController : ControllerBase
    {
        private readonly ServicoCidade _cidade;

        public CidadeController(ServicoCidade clienteService)
        {
            _cidade = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Cidade>> Get() =>
            _cidade.Get();


        [HttpGet("{id:length(24)}", Name = "GetCidade")]
        public ActionResult<Cidade> Get(string id)
        {
            var cidade = _cidade.Get(id);

            if (cidade == null)
            {
                return NotFound();
            }

            return cidade;
        }

        [HttpGet("nomeCidade/{nomeCidade}/estado/{estado}")]
        public ActionResult<Cidade> GetNomeCidadeEstado(string nomeCidade, string estado)
        {
            var cidade = _cidade.GetNomeCidadeEstado(nomeCidade, estado);

            if (cidade == null)
            {
                return NotFound();
            }

            return cidade;
        }

        [HttpPost]
        public ActionResult<Cidade> Create(Cidade cidade)
        {
            if(_cidade.Create(cidade) == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetCidade", new { id = cidade.Id.ToString() }, cidade);
        }

        [HttpPut("{id:length(24)}")]

        public IActionResult Update(string id, Cidade cidadeModificacao)
        {
            var cidade = _cidade.Get(id);

            if (cidade == null)
            {
                return NotFound();
            }
            if (_cidade.Update(id, cidadeModificacao) == null)
            {
                return BadRequest();
            }
            _cidade.Update(id, cidadeModificacao);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var cidade = _cidade.Get(id);

            if (cidade == null)
            {
                return NotFound();
            }

            _cidade.Remove(cidade);

            return NoContent();
        }
    }
}
