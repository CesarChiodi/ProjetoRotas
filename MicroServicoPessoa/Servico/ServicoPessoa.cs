using System.Collections.Generic;
using MicroServicoPessoa.Configuracao;
using MicroServicoPessoa.Model;
using MongoDB.Driver;

namespace MicroServicoPessoa.Servico
{
    public class ServicoPessoa
    {
        private readonly IMongoCollection<Pessoa> _pessoa;

        public ServicoPessoa(IPessoaMicroServico settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _pessoa = database.GetCollection<Pessoa>(settings.PessoaCollectionName);
        }

        public List<Pessoa> Get() =>
            _pessoa.Find(pessoa => true).ToList();

        public Pessoa Get(string id) =>
            _pessoa.Find(pessoa => pessoa.Id == id).FirstOrDefault();

        public Pessoa GetNomePessoa(string nomePessoa) =>
            _pessoa.Find(pessoa => pessoa.NomePessoa == nomePessoa).FirstOrDefault();

        public List<Pessoa> GetAtividade(string atividade) =>
            _pessoa.Find(pessoa => pessoa.NomePessoa == atividade).ToList();

        public Pessoa Create(Pessoa pessoa)
        {
            if (GetNomePessoa(pessoa.NomePessoa) != null)
            {
                return null;
            }

            pessoa.Ativo = false;

            _pessoa.InsertOne(pessoa);
            return pessoa;
        }

        public Pessoa Update(string id, Pessoa pessoaModificacao)
        {
            var verificaPessoa = GetNomePessoa(pessoaModificacao.NomePessoa);

            if (verificaPessoa != null && verificaPessoa.Id != pessoaModificacao.Id)
            {
                return null;
            }

            _pessoa.ReplaceOne(pessoa => pessoa.Id == id, pessoaModificacao);

            return pessoaModificacao;
        }

        public Pessoa UpdateNomePessoa(string nomePessoa, Pessoa pessoaModificacao)
        {
            var verificaPessoa = GetNomePessoa(pessoaModificacao.NomePessoa);

            if (verificaPessoa != null && verificaPessoa.Id != pessoaModificacao.Id)
            {
                return null;
            }

            _pessoa.ReplaceOne(pessoa => pessoa.NomePessoa == nomePessoa, pessoaModificacao);

            return pessoaModificacao;
        }

        public void Remove(Pessoa pessoaModificacao) =>
            _pessoa.DeleteOne(pessoa => pessoa.Id == pessoaModificacao.Id);

        public void Remove(string id) =>
            _pessoa.DeleteOne(pessoa => pessoa.Id == id);
    }
}
