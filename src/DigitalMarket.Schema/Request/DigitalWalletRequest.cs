using DigitalMarket.Base.Schema;

namespace DigitalMarket.Schema.Request
{
    public class DigitalWalletRequest : BaseRequest
    {
        public long UserId { get; set; }
        public decimal? PointBalance { get; set; }
    }
}
