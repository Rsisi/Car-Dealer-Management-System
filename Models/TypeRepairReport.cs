using System;
namespace team011.Models
{
    public class TypeRepairReport
    {
        public string vehicle_type{ get; set; }
        public int repair_count { get; set; }
        public decimal total_parts_cost { get; set; }
        public decimal total_labor_charge { get; set; }
        public decimal total_repair_cost { get; set; }
    }
}
