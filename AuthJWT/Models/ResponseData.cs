using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models
{
    public class ResponseData
    {
        public ResponseData()
        {
            Data = null;
        }
        
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
