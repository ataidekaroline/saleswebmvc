using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Enter a valid email")]
        public string Email { get; set; }
        [Display(Name = "Birth Date")]


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }


        [Required]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name ="Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }
        
        public Department Department { get; set; }
        
        public int DepartmentId { get; set; }
        
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {
        }
        public Seller(string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales (DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }

        public static implicit operator Seller(string v)
        {
            throw new NotImplementedException();
        }
    }
}
