using DigitalMarket.Base.Schema;
using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Request
{
    public class ProductRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }
        public decimal PointPercentage { get; set; }
        public decimal MaxPoint { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
