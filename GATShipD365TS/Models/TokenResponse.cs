using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATShipD365TS.Models
{
    public class TokenResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
        // Add other properties if needed (e.g., token type, expiration time)
    }
}
