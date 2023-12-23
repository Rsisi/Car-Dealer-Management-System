using System;
namespace team011.Models
{
    public class Part
    {

        public string VIN { get; set; }
        public DateTime start_date { get; set; }
        public string part_number { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string vendor { get; set; }
    }
}
