using AspNetCoreCrudMongo.Data;
using AspNetCoreCrudMongo.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AspNetCoreCrudMongo.Controllers
{
    public class ClienteController : Controller
    {
        private readonly MongoDbService _mongo;
        private readonly IMongoCollection<Cliente> _clientes;

        public ClienteController(MongoDbService mongo)
        {
            _mongo = mongo;
            _clientes = mongo.Database?.GetCollection<Cliente>("cliente");
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clientes.Find(x => true).ToListAsync();
            return View(clientes);
        }

        //GET: Clientes/Details/5
        public async Task<IActionResult> Details(string? id) { 
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _clientes.Find(x => x.Id == id).FirstOrDefaultAsync();
            
            if (clientes == null) { 
               return NotFound();
            }

            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Nome, Senha")] Cliente cliente) //bind são os campos que podem ser enviados
        {
            if (ModelState.IsValid) //verifica as verificações da model, tipo required, ou tamanho mínimo
            {
                await _clientes.InsertOneAsync(cliente);
                return RedirectToAction(nameof(Index)); //redireciona para outro método
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5

        public async Task<IActionResult> Edit(string? id) { 
            if(id == null) {
                return NotFound(); 
            }
            var cliente = await _clientes.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (cliente == null) {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Senha")] Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();

            if (ModelState.IsValid) //verifica os inputs da view
            {
                // ✅ VERIFICA resultado do ReplaceOne
                var filter = Builders<Cliente>.Filter.Eq(x => x.Id, id);
                var result = await _clientes.ReplaceOneAsync(filter, cliente);

                if (result.ModifiedCount == 0)  // ← NENHUM documento alterado
                {
                    if (!await ClienteExists(id)) return NotFound();
                    ModelState.AddModelError("", "Cliente foi alterado por outro usuário.");
                }
                else
                {
                    return RedirectToAction(nameof(Index));  // ✅ MVC correto!
                }
            }

            return View(cliente);
        }

        // GET: Estudantes/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) { return NotFound(); }

            var cliente = await _clientes.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (cliente == null) { return NotFound(); }

            return View(cliente);

        }

        //deleta de fato
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var filter = Builders<Cliente>.Filter.Eq(x => x.Id, id);
            await _clientes.DeleteOneAsync(filter);  // ← Simples assim!
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ClienteExists(string id)
        {
            var filter = Builders<Cliente>.Filter.Eq(x => x.Id, id);
            return await _clientes.Find(filter).AnyAsync();  // ← true/false se existe
        }


    }
}
