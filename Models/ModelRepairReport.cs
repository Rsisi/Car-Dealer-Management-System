using System;
namespace team011.Models
{
    public class ModelRepairReport
    {
        public string manufacturer { get; set; }
        public string vehicle_type { get; set; }
        public string model_name{ get; set; }
        public int repair_count { get; set; }
        public decimal total_parts_cost { get; set; }
        public decimal total_labor_charge { get; set; }
        public decimal total_repair_cost { get; set; }
    }
}
