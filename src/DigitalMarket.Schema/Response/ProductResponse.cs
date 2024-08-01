using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class ProductResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }
        public decimal PointPercentage { get; set; }
        public decimal MaxPoint { get; set; }
        public bool IsActive { get; set; }
    }
}
