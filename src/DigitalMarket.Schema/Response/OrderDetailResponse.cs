using DigitalMarket.Base.Schema;
using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Response
{
    public class OrderDetailResponse : BaseResponse
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public long[] CategoryIds { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PointAmount { get; set; }
        
    }
}
