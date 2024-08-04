using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Response
{
    public class ProductWithCategoryResponse : BaseResponse
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductStockCount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductPointPercentage { get; set; }
        public decimal ProductMaxPoint { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
