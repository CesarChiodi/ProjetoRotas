using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Servico
{
    public class VerificaCidade
    {
        public static async Task<List<Cidade>> EncontraTodasCidades()
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44346/api/Cidade");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                List<Cidade> cidade = JsonConvert.DeserializeObject<List<Cidade>>(responseBody);

                return cidade;
            }
            catch (HttpRequestException excecao)
            {
                return null;
            }
        }

        public static async Task<Cidade> EncontraCidadeUnica(string id)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44346/api/Cidade/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Cidade cidade = JsonConvert.DeserializeObject<Cidade>(responseBody);

                return cidade;
            }
            catch (HttpRequestException excecao)
            {
                return null;
            }
        }

        public static async Task<Cidade> EncontraCidadeNomeEstado(string nomeCidade, string estado)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44346/api/Cidade/nomeCidade/" + nomeCidade + "/estado/" + estado);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Cidade cidade = JsonConvert.DeserializeObject<Cidade>(responseBody);

                return cidade;
            }
            catch (HttpRequestException excecao)
            {
                return null;
            }
        }

        public static async void GerarCidade(Cidade cidadeModificacao)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(cidadeModificacao);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44346/api/Cidade/", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void AtualizarCidade(string id, Cidade cidadeModificacao)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(cidadeModificacao);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44346/api/Cidade/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void RemoverCidade(string id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44346/api/Cidade/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
