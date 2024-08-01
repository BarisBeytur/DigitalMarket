using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("Coupon", Schema = "dbo")]
    public class Coupon : BaseEntity
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
    }
}
