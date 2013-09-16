using System;
using System.Collections.Generic;

namespace SugarRest.Model
{
    public class EmailAddress
    {
        public string email_address { get; set; }
        public string opt_out { get; set; }
        public string invalid_email { get; set; }
        public string primary_address { get; set; }
    }
}
