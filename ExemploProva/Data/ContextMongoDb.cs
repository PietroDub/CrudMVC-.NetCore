using ExemploProva.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ExemploProva.Data
{
    public class ContextMongoDb
    {
        private readonly IMongoDatabase _database;

        public ContextMongoDb(IOptions<MongoSettings> settings)
        {
            var mongoSettings = settings.Value;
            var client = new MongoUrl(mongoSettings.ConnectionString);
            var clientSettings = MongoClientSettings.FromUrl(client);
            var cliente = new MongoClient(clientSettings);
            _database = cliente.GetDatabase(mongoSettings.Database);
        }

        public IMongoCollection<Cliente> Clientes =>
            _database.GetCollection<Cliente>("Clientes");
    }
}
