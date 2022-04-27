using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Servico
{
    public class VerificaTime
    {
        public static async Task<List<Time>> EncontraTodosTimes()
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44389/api/Time");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                List<Time> time = JsonConvert.DeserializeObject<List<Time>>(responseBody);

                return time;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Time> EncontraTimeUnico(string id)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44389/api/Time/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Time time = JsonConvert.DeserializeObject<Time>(responseBody);

                return time;
            }
            catch
            {
                return null;
            }
        }

        public static async void GerarTime(Time timeModificacao)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(timeModificacao);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44389/api/Time/", content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void AtualizarTime(string id, Time timeModificacao)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(timeModificacao);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44389/api/Time/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw;
            }
        }

        public static async void RemoverTime(string id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44389/api/Time/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
