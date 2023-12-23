using System;
namespace team011.Models
{
    public class SalesByManufacturerReport
    {
        public string manufacturer { get; set; }
        public int thirtyDaysSales { get; set; }
        public int lastYearSales { get; set; }
        public int overallSales { get; set; }
    }
}
