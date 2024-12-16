using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly SellersService _sellersService;

        public HomeController(SellersService sellersService)
        {
            _sellersService = sellersService;
        }

        public IActionResult Index()
        {
            var sellers = _sellersService.FindAll();  // Obt�m os dados
            return View(sellers);  // Passa os dados para a View
        }
    }
}
