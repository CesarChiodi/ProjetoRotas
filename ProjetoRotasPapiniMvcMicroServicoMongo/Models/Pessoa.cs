using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Models
{
    public class Pessoa
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("nomePessoa")]
        public string NomePessoa { get; set; }
        [JsonProperty("ativo")]
        public bool Ativo { get; set; }
    }
}
