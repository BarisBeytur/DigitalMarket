using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Request
{
    public class ProductCategoryRequest : BaseRequest
    {
        public long CategoryId { get; set; }
        public long ProductId { get; set; }
    }
}
