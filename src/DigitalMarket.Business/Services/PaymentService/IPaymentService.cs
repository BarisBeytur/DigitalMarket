using DigitalMarket.Base.Response;
using DigitalMarket.Schema.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponse<bool>> GetPayment(long userId, PaymentRequest request);
    }
}
