using DigitalMarket.Base.Entity;
using Microsoft.AspNetCore.Identity;

namespace DigitalMarket.Data.Domain
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }

        public long DigitalWalletId { get; set; }
        public virtual DigitalWallet DigitalWallet { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
