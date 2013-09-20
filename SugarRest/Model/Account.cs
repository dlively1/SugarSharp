using System;

namespace SugarRest.Model
{
    public class Account : Company
    {

        public string account_type { get; set; }
        public string parent_id { get; set; }
        public string sic_code { get; set; }
        public bool? email_opt_out { get; set; }
        public bool? invalid_email { get; set; }
        public string campaign_id { get; set; }

    }
}
