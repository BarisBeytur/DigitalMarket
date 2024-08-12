using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("OrderDetail", Schema = "dbo")]
    public class OrderDetail : BaseEntity
    {
        public long OrderId { get; set; }
        public virtual Order Order { get; set; }

        public long ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
