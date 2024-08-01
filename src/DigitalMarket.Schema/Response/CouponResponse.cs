using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class CouponResponse : BaseResponse
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
    }
}
