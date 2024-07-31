using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain
{
    [Table("DigitalWallet", Schema = "dbo")]
    public class DigitalWallet : BaseEntity
    {
        public decimal PointBalance { get; set; }
        public decimal Balance { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
