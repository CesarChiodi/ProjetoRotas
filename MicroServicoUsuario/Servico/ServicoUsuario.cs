using System.Collections.Generic;
using MicroServicoUsuario.Configuracao;
using MicroServicoUsuario.Model;
using MongoDB.Driver;

namespace MicroServicoUsuario.Servico
{
    public class ServicoUsuario
    {
        private readonly IMongoCollection<Usuario> _usuario;

        public ServicoUsuario(IUsuarioMicroServico settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _usuario = database.GetCollection<Usuario>(settings.UsuarioCollectionName);
        }

        public List<Usuario> Get() =>
            _usuario.Find(usuario => true).ToList();

        public Usuario Get(string id) =>
            _usuario.Find(usuario => usuario.Id == id).FirstOrDefault();

        public Usuario GetNomeUsuario(string nomeUsuario) =>
            _usuario.Find(usuario => usuario.NomeUsuario == nomeUsuario).FirstOrDefault();

        public Usuario Create(Usuario usuario)
        {
            if (GetNomeUsuario(usuario.NomeUsuario) != null)
            {
                return null;
            }

            _usuario.InsertOne(usuario);
            return usuario;
        }

        public Usuario Update(string id, Usuario usuarioModificacao)
        {
            var verificaUsuario = GetNomeUsuario(usuarioModificacao.NomeUsuario);

            if (verificaUsuario != null && verificaUsuario.Id != usuarioModificacao.Id)
            {
                return null;
            }

            _usuario.ReplaceOne(usuario => usuario.Id == id, usuarioModificacao);

            return usuarioModificacao;
        }

        public Usuario UpdateNomeUsuario(string nomeUsuario, Usuario usuarioModificacao)
        {
            var verificaUsuario = GetNomeUsuario(usuarioModificacao.NomeUsuario);

            if (verificaUsuario != null && verificaUsuario.Id != usuarioModificacao.Id)
            {
                return null;
            }

            _usuario.ReplaceOne(usuario => usuario.NomeUsuario == nomeUsuario, usuarioModificacao);

            return usuarioModificacao;
        }

        public void Remove(Usuario usuarioModificacao) =>
            _usuario.DeleteOne(usuario => usuario.Id == usuarioModificacao.Id);

        public void Remove(string id) =>
            _usuario.DeleteOne(usuario => usuario.Id == id);
    }
}
