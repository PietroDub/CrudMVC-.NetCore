using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreCrudMongo.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
