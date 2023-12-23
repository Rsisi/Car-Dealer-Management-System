using System;
namespace team011.Models
{
    public class GrossOneIncomeRepairReport
    {

        public DateTime start_date { get; set; }
        public DateTime complete_date { get; set; }
        public string VIN { get; set; }
        public int odometer_value { get; set; }
        public decimal parts_cost { get; set; }
        public decimal labor_charge { get; set; }
        public decimal total_cost { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
