using System;
using System.Collections.Generic;

namespace SugarRest.Model
{
    public class Account : Bean
    {
        #region Account Properties

        public string account_type { get; set; }
        public string industry { get; set; }
        public string annual_revenue { get; set; } //@todo might be int
        public string rating { get; set; }
        public string ownership { get; set; }
        public string employees { get; set; } //@todo - check datatype
        public string ticker_symbol { get; set; }
        public List<EmailAddress> email { get; set; }
        public string email1 { get; set; }
        public string parent_id { get; set; }
        public string sic_code { get; set; }
        public bool email_opt_out { get; set; }
        public bool invalid_email { get; set; }
        public string campaign_id { get; set; }

        #endregion

        //@todo - check if these are in the company base class 
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
