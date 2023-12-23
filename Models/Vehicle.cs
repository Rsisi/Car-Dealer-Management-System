using System;
namespace team011.Models
{
    public class Vehicle
    {
        public string VIN { get; set; }
        public string description { get; set; }
        public int model_year { get; set; }
        public string model_name { get; set; }
        public decimal invoice_price { get; set; }
        public string manufacturer_name { get; set; }
        public string inv_writer_user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime add_date { get; set; }

        public string vehicle_type { get; set; }
        public decimal list_price { get; set; }
        public string vehicle_color  { get; set; }

        public bool isSold { get; set; }
        public decimal sold_price { get; set; }
        public DateTime sold_date { get; set; }
        public string sales_people_name { get; set; }

       
        public bool isRepaired { get; set; }

        public int number_of_doors { get; set; }
        public string roof_type { get; set; }
        public int back_seat_count { get; set; }
        public decimal cargo_capacity { get; set; }
        public string cargo_cover_type { get; set; }
        public int number_of_rear_axies { get; set; }
        public bool has_driver_side_back_door { get; set; }
        public int number_of_cupholder { get; set; }
        public string drivetrain_type { get; set; }

    }
}
