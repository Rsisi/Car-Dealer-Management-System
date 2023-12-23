using System;
namespace team011.Models
{
    public class BelowCostSalesReport
    {
        public string customer_name { get; set; }
        public DateTime transaction_date { get; set; }
        public decimal sold_price { get; set; }
        public decimal ratio { get; set; }
        public int model_year { get; set; }
        public string manufacturer_name { get; set; }
        public string model_name { get; set; }
        public decimal invoice_price { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string VIN { get; set; }
    }
}
