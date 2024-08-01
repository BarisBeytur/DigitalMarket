using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class ProductCategoryResponse : BaseResponse
    {
        public long CategoryId { get; set; }
        public long ProductId { get; set; }
    }
}
