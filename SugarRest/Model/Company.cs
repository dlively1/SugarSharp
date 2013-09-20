using System;
using System.Collections.Generic;

namespace SugarRest.Model
{
    /// <summary>
    /// Base company object
    /// </summary>
    public class Company : Bean
    {
        public List<EmailAddress> email { get; set; }
        public string email1 { get; set; }
        public string employees { get; set; }
        public string ticker_symbol { get; set; }
        public string ownership { get; set; }
        public string rating { get; set; }
        public string industry { get; set; }
        public string annual_revenue { get; set; }
        public string phone_fax { get; set; }
        public string billing_address_street { get; set; }
        public string billing_address_street_2 { get; set; }
        public string billing_address_street_3 { get; set; }
        public string billing_address_street_4 { get; set; }
        public string billing_address_city { get; set; }
        public string billing_address_state { get; set; }
        public string billing_address_postalcode { get; set; }
        public string billing_address_country { get; set; }
        public string phone_office { get; set; }
        public string phone_alternate { get; set; }
        public string website { get; set; }
        public string shipping_address_street { get; set; }
        public string shipping_address_street_2 { get; set; }
        public string shipping_address_street_3 { get; set; }
        public string shipping_address_street_4 { get; set; }
        public string shipping_address_city { get; set; }
        public string shipping_address_state { get; set; }
        public string shipping_address_postalcode { get; set; }
        public string shipping_address_country { get; set; }

    }
}
