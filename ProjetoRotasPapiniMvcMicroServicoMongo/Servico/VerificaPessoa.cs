using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;
using System.Text;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Servico
{
    public class VerificaPessoa
    {
        public static async Task <List<Pessoa>> EncontraTodasPessoa()
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage resposta = await client.GetAsync("https://localhost:44370/api/Pessoa");
                resposta.EnsureSuccessStatusCode();

                string responseBody = await resposta.Content.ReadAsStringAsync();
                List<Pessoa> pessoa = JsonConvert.DeserializeObject<List<Pessoa>>(responseBody);

                return pessoa;
            }
            catch (HttpRequestException excecao)
            { 
                throw;
            }
        }

        public static async Task<Pessoa> EncontraPessoaUnica(string id)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage resposta = await client.GetAsync("https://localhost:44370/api/Pessoa/" + id);
                resposta.EnsureSuccessStatusCode();

                string responseBody = await resposta.Content.ReadAsStringAsync();
                Pessoa pessoa = JsonConvert.DeserializeObject<Pessoa>(responseBody);

                return pessoa;
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void GerarPessoa(Pessoa pessoaMudanca)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(pessoaMudanca);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44370/api/Pessoa/", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void AtualizarPessoa(string id, Pessoa pessoaMudanca)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(pessoaMudanca);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44370/api/Pessoa/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void RemoverPessoa(string id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44370/api/Pessoa/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}


