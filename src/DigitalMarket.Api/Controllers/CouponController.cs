using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CouponCommands;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouponController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ApiResponse<CouponResponse>> Post(CouponRequest couponRequest)
    {
        var response = await _mediator.Send(new CreateCouponCommand(couponRequest));
        return response;
    }

    [HttpDelete]
    public async Task<ApiResponse> Delete(long id)
    {
        var response = await _mediator.Send(new DeleteCouponCommand { Id = id });
        return response;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<CouponResponse>> Put(long id, [FromBody] CouponRequest couponRequest)
    {
        var response = await _mediator.Send(new UpdateCouponCommand(id, couponRequest));
        return response;
    }
}

