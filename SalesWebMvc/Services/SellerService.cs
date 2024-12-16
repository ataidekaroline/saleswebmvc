using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using SalesWebMvc.Services;

namespace SalesWebMvc.Services
{
    public class SellersService
    {
        private readonly SalesWebMvcContext _context;

        public SellersService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();  // Retorna a lista de vendedores do banco de dados
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
        public Seller FindById(int id)
        {
            var seller = _context.Seller.Include(obj => obj.Department).FirstOrDefault(x => x.Id == id);
            if (seller == null)
            {
                throw new NotFoundException($"Seller with ID {id} not found");
            }
            return seller;
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();

        }
        public void update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }

    }
}
