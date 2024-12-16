using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly SellerService _sellersService;

        public HomeController(SellerService sellersService)
        {
            _sellersService = sellersService;
        }

        public IActionResult Index()
        {
            var sellers = _sellersService.FindAll();  // Obtém os dados
            return View(sellers);  // Passa os dados para a View
        }
    }
}
