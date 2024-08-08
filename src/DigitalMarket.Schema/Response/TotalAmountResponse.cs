namespace DigitalMarket.Schema.Response
{
    public class TotalAmountResponse
    {
        public decimal BasketAmount { get; set; }
        public decimal totalAmountAfterCouponApplied { get; set; }
        public decimal totalAmountAfterPointApplied { get; set; }
    }
}
