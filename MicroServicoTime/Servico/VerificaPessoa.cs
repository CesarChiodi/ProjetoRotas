using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MicroServicoTime.Model;
using Newtonsoft.Json;

namespace MicroServicoTime.Servico
{
    public class VerificaPessoa
    {
        public static async Task<Pessoa> GetPessoaId(string id)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44370/api/Pessoa/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var pessoa = JsonConvert.DeserializeObject<Pessoa>(responseBody);

                return pessoa;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Pessoa> GetPessoaNome(string nomePessoa)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44370/api/Pessoa/nomePessoa/" + nomePessoa);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var pessoa = JsonConvert.DeserializeObject<Pessoa>(responseBody);

                return pessoa;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<Pessoa>> GetPessoaStatus(string atividade)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44370/api/Pessoa/nomePessoa/" + atividade);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var pessoa = JsonConvert.DeserializeObject<List<Pessoa>>(responseBody);

                return pessoa;
            }
            catch
            {
                return null;
            }
        }

        public static async void UpdatePessoaAtividade(string nomePessoa, Pessoa pessoaModificacao)
        {
            HttpClient client = new HttpClient();

            try
            {
                var json = JsonConvert.SerializeObject(pessoaModificacao);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44370/api/Pessoa/nomePessoa/" + nomePessoa, content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }
    }
}
