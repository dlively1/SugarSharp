using System;
using System.Collections.Generic;

namespace SugarRest.Model
{
    public class TokenResponse
    {

        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }

    }
}
