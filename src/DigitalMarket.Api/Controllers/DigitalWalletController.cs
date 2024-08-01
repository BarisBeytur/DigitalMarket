using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigitalWalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DigitalWalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //todo: customer olustugunda dijital cuzdan otomatik olusturulacak.



        //todo : dijital cuzdan silinmez. kullanici silinirse o zaman dijital cuzdan da otomatik silinir.
        //[HttpDelete]
        //public async Task<ApiResponse> Delete(long id)
        //{
        //    var response = await _mediator.Send(new DeleteDigitalWalletCommand { Id = id });
        //    return response;
        //}

        [HttpPut("{id}")]
        public async Task<ApiResponse<DigitalWalletResponse>> Put(long id, [FromBody] DigitalWalletRequest digitalWalletRequest)
        {
            var response = await _mediator.Send(new UpdateDigitalWalletCommand(id, digitalWalletRequest));
            return response;
        }
    }
}
