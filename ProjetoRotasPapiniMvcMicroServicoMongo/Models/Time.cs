using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Models
{
    public class Time
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("nomeTime")]
        public string NomeTime { get; set; }
        [JsonProperty("pessoaTime")]
        public List<Pessoa> PessoaTime { get; set; }
        [JsonProperty("cidade")]
        public Cidade Cidade { get; set; }
    }
}
