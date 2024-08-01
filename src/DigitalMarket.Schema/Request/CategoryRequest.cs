using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Request
{
    public class CategoryRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
    }
}
