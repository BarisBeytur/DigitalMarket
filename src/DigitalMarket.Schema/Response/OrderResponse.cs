using DigitalMarket.Base.Schema;
using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Response
{
    public class OrderResponse : BaseResponse
    {
        public decimal TotalAmount { get; set; }
        public decimal? CouponAmount { get; set; }
        public decimal? PointAmount { get; set; }
        public decimal? BasketAmount { get; set; }
        public string? CouponCode { get; set; }
        public short Status { get; set; }
        public long OrderDetailId { get; set; }
        public long UserId { get; set; }

        public List<OrderDetailResponse> OrderDetails { get; set; }
    }
}
