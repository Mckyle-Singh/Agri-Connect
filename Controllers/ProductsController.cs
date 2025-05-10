using Microsoft.AspNetCore.Mvc;

namespace Agri_Connect.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


    }
}
