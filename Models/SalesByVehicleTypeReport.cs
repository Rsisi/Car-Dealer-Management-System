using System;
namespace team011.Models
{
    public class SalesByColorReport
    {
        public string color { get; set; }
        public int thirtyDaysSales { get; set; }
        public int lastYearSales { get; set; }
        public int overallSales { get; set; }
    }
}
