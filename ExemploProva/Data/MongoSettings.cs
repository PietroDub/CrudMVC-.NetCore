using System.ComponentModel.DataAnnotations;

namespace ExemploProva.Data
{
    public class MongoSettings
    {
        [Required]
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool IsSsl { get; set; }
    }
}
