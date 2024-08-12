using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;

namespace DigitalMarket.Business.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork<DigitalWallet> _digitalWalletUnitOfWork;

        public PaymentService(IUnitOfWork<DigitalWallet> digitalWalletUnitOfWork)
        {
            _digitalWalletUnitOfWork = digitalWalletUnitOfWork;
        }

        public async Task<ApiResponse<bool>> GetPayment(long userId, PaymentRequest request)
        {
            // odeme basariyla alindi.
            return new ApiResponse<bool>(true);
        }
    }
}
