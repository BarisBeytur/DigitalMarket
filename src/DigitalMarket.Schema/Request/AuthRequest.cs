using DigitalMarket.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Schema.Request
{
    public class AuthRequest : BaseRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
