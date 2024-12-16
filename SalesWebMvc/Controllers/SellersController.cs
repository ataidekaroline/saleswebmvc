using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Controllers;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellersService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellersService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var sellers = _sellerService.FindAll();

            if (sellers == null)
            {
                sellers = new List<Seller>(); // Para evitar passar null
            }

            return View(sellers);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                // Linha modificada: Redirecionamento para a página de erro com mensagem personalizada
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); // Linha 44
            }

            try
            {
                var obj = _sellerService.FindById(id.Value); // Linha 48
                return View(obj);
            }
            catch (NotFoundException e)
            {
                // Linha adicionada: Captura a exceção e redireciona para a página de erro
                return RedirectToAction(nameof(Error), new { message = e.Message }); // Linha 52
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _sellerService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                // Linha adicionada: Captura a exceção e redireciona para a página de erro
                return RedirectToAction(nameof(Error), new { message = e.Message }); // Linha 63
            }
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                // Linha modificada: Redirecionamento para a página de erro com mensagem personalizada
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); // Linha 70
            }

            try
            {
                var obj = _sellerService.FindById(id.Value); // Linha 74
                return View(obj);
            }
            catch (NotFoundException e)
            {
                // Linha adicionada: Captura a exceção e redireciona para a página de erro
                return RedirectToAction(nameof(Error), new { message = e.Message }); // Linha 78
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                // Linha modificada: Redirecionamento para a página de erro com mensagem personalizada
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); // Linha 84
            }

            try
            {
                var obj = _sellerService.FindById(id.Value); // Linha 88
                List<Department> departments = _departmentService.FindAll();
                SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
                return View(viewModel);
            }
            catch (NotFoundException e)
            {
                // Linha adicionada: Captura a exceção e redireciona para a página de erro
                return RedirectToAction(nameof(Error), new { message = e.Message }); // Linha 94
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                // Linha modificada: Redirecionamento para a página de erro com mensagem personalizada
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" }); // Linha 102
            }

            try
            {
                _sellerService.update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                // Linha adicionada: Captura a exceção e redireciona para a página de erro
                return RedirectToAction(nameof(Error), new { message = e.Message }); // Linha 110
            }
        }

        public IActionResult Error(string message)
        {
            // Método Error permanece inalterado
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
