namespace DigitalMarket.Schema.Request
{
    public class PaymentRequest
    {
        public string NameSurname { get; set; }
        public string CardNo { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
    }
}
