using DigitalMarket.Base.Schema;
using static DigitalMarket.Base.Enums.Enums;

namespace DigitalMarket.Schema.Request
{
    public class OrderRequest : BaseRequest
    {
        public decimal TotalAmount { get; set; }
        public decimal? CouponAmount { get; set; }
        public decimal? PointAmount { get; set; }
        public decimal? BasketAmount { get; set; }
        public string? CouponCode { get; set; }
        public short Status { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
