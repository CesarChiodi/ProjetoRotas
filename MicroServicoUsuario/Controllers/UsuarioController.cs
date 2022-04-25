using System.Collections.Generic;
using MicroServicoUsuario.Model;
using MicroServicoUsuario.Servico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoUsuario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ServicoUsuario _usuario;

        public UsuarioController(ServicoUsuario clienteService)
        {
            _usuario = clienteService;
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get() =>
            _usuario.Get();


        [HttpGet("{id:length(24)}", Name = "GetUsuario")]
        public ActionResult<Usuario> Get(string id)
        {
            var usuario = _usuario.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }


        [HttpGet("usuario/{nomeUsuario}")]
        public ActionResult<Usuario> GetNomeUsuario(string nomeUsuario)
        {
            var usuario = _usuario.GetNomeUsuario(nomeUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPost]
        public ActionResult<Usuario> Create(Usuario usuario)
        {
            if (_usuario.Create(usuario) == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetUsuario", new { id = usuario.Id.ToString() }, usuario);
        }

        [HttpPut("{id:length(24)}")]

        public IActionResult Update(string id, Usuario usuarioModificacao)
        {
            var usuario = _usuario.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            if (_usuario.Update(id, usuarioModificacao) == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var usuario = _usuario.Get(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _usuario.Remove(usuario);

            return NoContent();
        }
    }
}
