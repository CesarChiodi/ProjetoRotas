using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MicroServicoCidade.Configuracao;
using MicroServicoCidade.Model;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MicroServicoCidade.Servico
{
    public class ServicoCidade
    {
        private readonly IMongoCollection<Cidade> _cidade;

        public ServicoCidade(ICidadeMicroServicoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _cidade = database.GetCollection<Cidade>(settings.CidadeCollectionName);
        }

        public List<Cidade> Get() =>
            _cidade.Find(city => true).ToList();

        public Cidade Get(string id) =>
            _cidade.Find(cidade => cidade.Id == id).FirstOrDefault();

        public Cidade GetNomeCidadeEstado(string nome, string estado) =>
            _cidade.Find(cidade => cidade.NomeCidade == nome && cidade.Estado == estado).FirstOrDefault();

        public Cidade Create(Cidade cidade)
        {

            if(GetNomeCidadeEstado(cidade.NomeCidade, cidade.Estado) != null)
            {
                return null;
            }

            _cidade.InsertOne(cidade);

            return cidade;
        }

        public Cidade Update(string id, Cidade cidadeModificacao)
        {
            Cidade verificaCidade = GetNomeCidadeEstado(cidadeModificacao.NomeCidade, cidadeModificacao.Estado);

            if (verificaCidade != null && verificaCidade.Id != cidadeModificacao.Id)
            {
                return null;
            }

            _cidade.ReplaceOne(cidade => cidade.Id == id, cidadeModificacao);

            return cidadeModificacao;
        }

        public void Remove(Cidade cidadeModificacao) =>
            _cidade.DeleteOne(cidade => cidade.Id == cidadeModificacao.Id);

        public void Remove(string id) =>
            _cidade.DeleteOne(cidade => cidade.Id == id);
    }
}
