using System;
namespace team011.Models
{
    public class Customer
    {
       
        public int customerID { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string street_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public bool isIndividual { get; set; }
        public string driver_license { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string tax_ID { get; set; }
        public string contact_title { get; set; }
        public string contact_name { get; set; }
        public string business_name { get; set; }
        public string lastURL { get; set; }
        public string customerName { get; set; }

        public string concat_customerName_identityID { get; set; }
    }
}
