using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MicroServicoCidade.Model
{
    public class Cidade
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [JsonProperty("nomeCidade")]
        public string NomeCidade { get; set; }
        [JsonProperty("estado")]
        public string Estado { get; set; }
    }
}
