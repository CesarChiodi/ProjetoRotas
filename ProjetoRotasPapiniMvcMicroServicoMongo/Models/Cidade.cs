using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Models
{
    public class Cidade
    {
        [Key]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("nomeCidade")]
        public string NomeCidade { get; set; }
        [JsonProperty("estado")]
        public string Estado { get; set; }
    }
}
