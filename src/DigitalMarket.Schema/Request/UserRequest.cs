using DigitalMarket.Base.Schema;
using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Request
{
    public class UserRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
    }
}
