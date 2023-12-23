using System;
namespace team011.Models
{
    public class MonthlySalesReport
    {
        public int year { get; set; }

        public int month { get; set; }
        public int totalVehicleSold { get; set; }
        public decimal totalNetIncome { get; set; }
        public decimal SalesIncome { get; set; }
        public decimal Ratio { get; set; }

    }
}
