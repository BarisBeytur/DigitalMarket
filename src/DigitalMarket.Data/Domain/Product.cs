using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("Product", Schema = "dbo")]
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }
        public decimal PointPercentage { get; set; }
        public decimal MaxPoint { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
