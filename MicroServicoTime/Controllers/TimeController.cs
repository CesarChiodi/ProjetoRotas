using System.Collections.Generic;
using System.Threading.Tasks;
using MicroServicoTime.Model;
using MicroServicoTime.Servico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly ServicoTime _time;

        public TimeController(ServicoTime clienteService)
        {
            _time = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Time>> Get() =>
            _time.Get();

        //[HttpGet]
        //public async Task<ActionResult<Cidade>> Get()
        //{
        //    var cidade = await _time.GetNomeCidadeEstado("Marília", "sp");

        //    if (cidade == null)
        //    {
        //        return NotFound();
        //    }

        //    return cidade;

        //}

        [HttpGet("{id:length(24)}", Name = "GetTime")]
        public ActionResult<Time> Get(string id)
        {
            var time = _time.Get(id);

            if (time == null)
            {
                return NotFound();
            }

            return time;
        }

        [HttpGet("nomeTime/{nomeTime}")]
        public ActionResult<Time> GetNome(string nomeTime)
        {
            var time = _time.GetNomeTime(nomeTime);

            if (time == null)
                return NotFound();

            return time;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Time time)
        {
            if (await _time.Create(time) == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetTime", new { id = time.Id.ToString() }, time);
        }

        [HttpPut("{id:length(24)}")]

        public async Task<IActionResult> Update(string id, Time timeModificacao)
        {
            var time = _time.Get(id);

            if (time == null)
            {
                return NotFound();
            }

            if (await _time.Update(id, timeModificacao) == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var time = _time.Get(id);

            if (time == null)
            {
                return NotFound();
            }

            _time.Remove(time.Id);

            return NoContent();
        }
    }
}
