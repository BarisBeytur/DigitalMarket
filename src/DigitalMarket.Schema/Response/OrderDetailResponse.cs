using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class OrderDetailResponse : BaseResponse
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PointAmount { get; set; }
    }
}
