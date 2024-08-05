using DigitalMarket.Base.Schema;
using static DigitalMarket.Base.Enums.Enums;

namespace DigitalMarket.Schema.Request
{
    public class OrderRequest : BaseRequest
    {
        public long UserId { get; set; }
        public string? CouponCode { get; set; }
        public bool IsActive { get; set; }
    }
}
