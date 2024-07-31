using DigitalMarket.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalMarket.Data.Domain;

[Table("User", Schema = "dbo")]
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public bool Status { get; set; }

    public long DigitalWalletId { get; set; }
    public virtual DigitalWallet DigitalWallet { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
