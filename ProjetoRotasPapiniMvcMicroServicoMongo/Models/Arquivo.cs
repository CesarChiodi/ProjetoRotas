using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Models
{
    public class Arquivo
    {
        public int Id { get; set; }

        [DisplayName("NomeArquivo")]
        public string FileName { get; set; }

        [NotMapped]
        [DisplayName("Arquivo")]
        public IFormFile File { get; set; }
    }
}
