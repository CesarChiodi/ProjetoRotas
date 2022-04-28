using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MicroServicoTime.Configuracao;
using MicroServicoTime.Model;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MicroServicoTime.Servico
{
    public class ServicoTime
    {
        private readonly IMongoCollection<Time> _time;
        public ServicoTime(ITimeMicroServico settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _time = database.GetCollection<Time>(settings.TimeCollectionName);
        }
        public List<Time> Get() =>
            _time.Find(time => true).ToList();

        public Time Get(string id) =>
            _time.Find(time => time.Id == id).FirstOrDefault();

        public Time GetNomeTime(string nome) =>
            _time.Find(time => time.NomeTime == nome).FirstOrDefault();

      
        public async Task<Time> Create(Time time)
        {
            if (GetNomeTime(time.NomeTime) != null)
            {
                return null;
            }

            var cidade = await VerificaCidade.GetNomeCidadeEstado(time.Cidade.NomeCidade, time.Cidade.Estado);

            if (cidade == null)
            {
                return null;
            }

            if (time.PessoaTime == null)
            {
                return null;
            }

            List<Pessoa> listaPessoaTime = new List<Pessoa>();

            foreach (var pessoaTimeId in time.PessoaTime)
            {
                var pessoaTime = await VerificaPessoa.GetPessoaNome(pessoaTimeId.NomePessoa);

                if (pessoaTime == null)
                {
                    return null;
                }

                else if (pessoaTime.Ativo == true)
                {
                    return null;
                }

                else
                {
                    pessoaTime.Ativo = true;
                    VerificaPessoa.UpdatePessoaAtividade(pessoaTime.NomePessoa, pessoaTime);
                    listaPessoaTime.Add(pessoaTime);
                }
            }

            time.PessoaTime = listaPessoaTime;
            time.Cidade = cidade;

            _time.InsertOne(time);

            return time;
        }

        public async Task<Time> Update(string id, Time timeModificacao)
        {
            var cidade = await VerificaCidade.GetCidadeId(timeModificacao.Cidade.Id);

            if (cidade == null)
            {
                return null;
            }

            if (timeModificacao.PessoaTime == null)
            {
                return null;
            }

            List<Pessoa> listaPessoaTime = new();

            foreach (var pessoaTimeId in timeModificacao.PessoaTime)
            {
                var pessoaTime = await VerificaPessoa.GetPessoaNome(pessoaTimeId.NomePessoa);

                if (pessoaTime == null)
                {
                    return null;
                }

                else
                {
                    pessoaTime.Ativo = true;
                    VerificaPessoa.UpdatePessoaAtividade(pessoaTime.NomePessoa, pessoaTime);
                    listaPessoaTime.Add(pessoaTime);
                }

            }

            if (listaPessoaTime.Count == 0)
            {
                return null;
            }

            timeModificacao.PessoaTime = listaPessoaTime;
            timeModificacao.Cidade = cidade;

            _time.ReplaceOne(time => time.Id == id, timeModificacao);

            return timeModificacao;
        }

        public void Remove(Time timeModificacao) =>
            _time.DeleteOne(time => time.Id == timeModificacao.Id);

        public void Remove(string id) =>
            _time.DeleteOne(time => time.Id == id);
    }
}
