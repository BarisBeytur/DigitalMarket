using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CouponCommands;
using DigitalMarket.Business.CQRS.Queries.CouponQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouponController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<CouponResponse>>> GetAll()
    {
        var response = await _mediator.Send(new GetAllCouponQuery());
        return response;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<CouponResponse>> Get(long id)
    {
        var response = await _mediator.Send(new GetCouponByIdQuery(id));
        return response;
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

