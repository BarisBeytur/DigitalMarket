using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Request
{
    public class CouponRequest : BaseRequest
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
    }
}
