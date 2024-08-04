using DigitalMarket.Data.Domain;

namespace DigitalMarket.Schema.Response
{
    public class PointBalanceResponse
    {
        public decimal? PointBalance { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public long UserDigitalWalletId { get; set; }
    }
}
