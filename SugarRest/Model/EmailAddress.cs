using System;

namespace SugarRest.Model
{
    public class EmailAddress
    {
        public string email_address { get; set; }
        public bool? opt_out { get; set; }
        public bool? invalid_email { get; set; }
        public string primary_address { get; set; }
    }
}
