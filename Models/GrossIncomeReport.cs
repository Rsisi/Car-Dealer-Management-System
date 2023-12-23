using System;
namespace team011.Models
{
    public class GrossIncomeReport
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime FirstServiceDate { get; set; }
        public DateTime LatestServiceDate { get; set; }
        public int NumOfRepair { get; set; }

        public int NumOfSale { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
