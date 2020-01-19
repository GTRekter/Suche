using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Authentication
{
    public class TokenResponse
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
    }
}
