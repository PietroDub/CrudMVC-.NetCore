using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreCrudMongo.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonElement("Id_Cliente"), BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("Nome_Cliente"), BsonRepresentation(BsonType.String)]
        [[Required(ErrorMessage = "Nome obrigatório")]
        public string? Nome { get; set; }

        [BsonElement("Senha_Cliente"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Senha obrigatório")]
        public string? Senha { get; set; }
        
    }
}
