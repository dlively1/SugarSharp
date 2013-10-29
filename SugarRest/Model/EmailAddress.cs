using System;

namespace SugarRest.Model
{
    public class EmailAddress
    {
        public string email_address { get; set; }
        public string opt_out { get; set; } //these seem to comeback as strings in the service ??
        public string invalid_email { get; set; } //and this
        public string primary_address { get; set; }
    }
}
