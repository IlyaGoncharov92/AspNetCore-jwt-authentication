using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }
}
