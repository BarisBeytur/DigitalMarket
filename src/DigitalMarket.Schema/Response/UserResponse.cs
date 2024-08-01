using DigitalMarket.Base.Schema;
using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Response
{
    public class UserResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public long DigitalWalletId { get; set; }
    }
}
