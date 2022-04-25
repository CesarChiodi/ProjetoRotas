using System.Net.Http;
using System.Threading.Tasks;
using MicroServicoTime.Model;
using Newtonsoft.Json;

namespace MicroServicoTime.Servico
{
    public class VerificaCidade
    {
        public static async Task<Cidade> GetCidadeId(string id)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44346/api/Cidade/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var cidade = JsonConvert.DeserializeObject<Cidade>(responseBody);

                return cidade;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Cidade> GetNomeCidadeEstado(string nomeCidade, string estado)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44346/api/Cidade/nomeCidade/" + nomeCidade + "/estado/" + estado);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var cidade = JsonConvert.DeserializeObject<Cidade>(responseBody);

                return cidade;
            }
            catch
            {
                return null;
            }
        }
    }
}
