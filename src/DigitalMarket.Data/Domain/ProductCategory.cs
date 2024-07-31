using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("ProductCategory", Schema = "dbo")]
    public class ProductCategory : BaseEntity
    {
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
