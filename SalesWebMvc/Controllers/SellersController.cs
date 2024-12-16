using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellersService _sellersService;

        public SellersController(SellersService sellersService)
        {
            _sellersService = sellersService;
        }

        public IActionResult Index()
        {
            var sellers = _sellersService.FindAll();

            if (sellers == null)
            {
                sellers = new List<Seller>(); // Para evitar passar null
            }

            return View(sellers);
        }
    }
}

