using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Response
{
    public class PointBalanceResponse
    {
        public decimal? PointBalance { get; set; }
        public long UserId { get; set; }
    }
}
