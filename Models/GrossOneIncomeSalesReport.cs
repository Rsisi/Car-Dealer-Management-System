using System;
namespace team011.Models
{
    public class GrossOneIncomeSalesReport
    {
        public DateTime TransactionDate { get; set; }
        public decimal SoldPrice { get; set; }
        public string VIN { get; set; }
        public int ModelYear{ get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
    }
}
