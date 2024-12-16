using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Controllers;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using NuGet.Protocol.Plugins;

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
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); 
            }

            try
            {
                var obj = _sellerService.FindById(id.Value); 
                return View(obj);
            }
            catch (NotFoundException e)
            {
                
                return RedirectToAction(nameof(Error), new { message = e.Message }); 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Seller seller)
        {
            
            try
            {
                _sellerService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                
                return RedirectToAction(nameof(Error), new { message = e.Message }); 
            }
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
               
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); 
            }

            try
            {
                var obj = _sellerService.FindById(id.Value); 
                return View(obj);
            }
            catch (NotFoundException e)
            {
                
                return RedirectToAction(nameof(Error), new { message = e.Message }); 
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); 
            }

            try
            {
                var obj = _sellerService.FindById(id.Value);
                List<Department> departments = _departmentService.FindAll();
                SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
                return View(viewModel);
            }
            catch (NotFoundException e)
            {
               
                return RedirectToAction(nameof(Error), new { message = e.Message }); 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" }); 
            }

            try
            {
                _sellerService.update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                
                return RedirectToAction(nameof(Error), new { message = e.Message }); 
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
