using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjetoRotasPapiniMvcMicroServicoMongo.Models;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Servico
{
    public class VerificaUsuario
    {
        public static async Task<List<Usuario>> EncontraTodosUsuarios()
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage resposta = await client.GetAsync("https://localhost:44356/api/Usuario");
                resposta.EnsureSuccessStatusCode();

                string responseBody = await resposta.Content.ReadAsStringAsync();
                List<Usuario> usuario = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);

                return usuario;
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async Task<Usuario> EncontraUsuarioUnico(string id)
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage resposta = await client.GetAsync("https://localhost:44356/api/Usuario/" + id);
                resposta.EnsureSuccessStatusCode();

                string responseBody = await resposta.Content.ReadAsStringAsync();
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(responseBody);

                return usuario;
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async Task<Usuario> EncontraNomeUsuario(string nomeUsuario)
        {
            HttpClient client = new HttpClient();

            try
            {
                //https://localhost:44356/api/Usuario/usuario/cesar
                HttpResponseMessage resposta = await client.GetAsync("https://localhost:44356/api/Usuario/usuario/" + nomeUsuario);
                resposta.EnsureSuccessStatusCode();

                string responseBody = await resposta.Content.ReadAsStringAsync();
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(responseBody);

                return usuario;
            }
            catch (HttpRequestException excecao)
            {
                return null;
            }
        }

        public static async void GerarUsuario(Usuario usuarioMudanca)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(usuarioMudanca);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44356/api/Usuario/", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void AtualizarUsuario(string id, Usuario usuarioMudanca)
        {
            HttpClient client = new HttpClient();

            try
            {
                string json = JsonConvert.SerializeObject(usuarioMudanca);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("https://localhost:44356/api/Usuario/" + id, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException excecao)
            {
                throw;
            }
        }

        public static async void RemoverUsuario(string id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync("https://localhost:44356/api/Usuario/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
