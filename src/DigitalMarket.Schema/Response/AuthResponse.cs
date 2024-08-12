using DigitalMarket.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Schema.Response
{
    public class AuthResponse : BaseResponse
    {
        public DateTime ExpireTime { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
    }
}
