using DigitalMarket.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Schema.Request
{
    public class ChangePasswordRequest : BaseRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
