using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public SaleStatus Status { get; set; }
        public Seller Seller { get; set; }

        public SalesRecord()
        {
        }

        public SalesRecord(DateTime date, double amount, SaleStatus status, Seller seller)
        {
            
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
