using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("Order", Schema = "dbo")]
    public class Order : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal? CouponAmount { get; set; }
        public decimal? PointAmount { get; set; }
        public decimal? BasketAmount { get; set; }
        public string? CouponCode { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
