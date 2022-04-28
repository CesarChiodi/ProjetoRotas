using System.Collections.Generic;
using MicroServicoPessoa.Model;
using MicroServicoPessoa.Servico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoPessoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly ServicoPessoa _pessoa;

        public PessoaController(ServicoPessoa clienteService)
        {
            _pessoa = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Pessoa>> Get() =>
            _pessoa.Get();


        [HttpGet("{id:length(24)}", Name = "GetPessoa")]
        public ActionResult<Pessoa> Get(string id)
        {
            var pessoa = _pessoa.Get(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        [HttpGet("nomePessoa/{nome}")]
        public ActionResult<Pessoa> GetNomePessoa(string nome)
        {
            var pessoa = _pessoa.GetNomePessoa(nome);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        [HttpGet("atividade/{atividade}")]
        public ActionResult<List<Pessoa>> GetAtividade(string atividade)
        {
            var pessoa = _pessoa.GetAtividade(atividade);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }


        [HttpPost]
        public ActionResult<Pessoa> Create(Pessoa pessoa)
        {
            if (_pessoa.Create(pessoa) == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetPessoa", new { id = pessoa.Id.ToString() }, pessoa);
        }

        [HttpPut("{id:length(24)}")]

        public IActionResult Update(string id, Pessoa pessoaModificacao)
        {
            var pessoa = _pessoa.Get(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            if (_pessoa.Update(id, pessoaModificacao) == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("nomePessoa/{nomePessoa}")]
        public IActionResult UpdateNomePessoa(string nomePessoa, Pessoa pessoaModificacao)
        {
            var pessoa = _pessoa.GetNomePessoa(nomePessoa);

            if (pessoa == null)
            {
                return NotFound();
            }

            if (_pessoa.UpdateNomePessoa(nomePessoa, pessoaModificacao) == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var pessoa = _pessoa.Get(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            _pessoa.Remove(pessoa);

            return NoContent();
        }
    }
}
