using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class CategoryResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
    }
}
