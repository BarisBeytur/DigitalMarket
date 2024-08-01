using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Request
{
    public class DigitalWalletRequest : BaseRequest
    {
        public decimal? PointBalance { get; set; }
        public decimal Balance { get; set; }
        public long UserId { get; set; }
    }
}
