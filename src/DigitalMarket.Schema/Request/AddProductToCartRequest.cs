using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Schema.Request
{
    public class AddProductToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
