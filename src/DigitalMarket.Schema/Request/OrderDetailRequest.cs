using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Request
{
    public class OrderDetailRequest : BaseRequest
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
