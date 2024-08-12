using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CouponCommands;
using DigitalMarket.Business.CQRS.Queries.CouponQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<IEnumerable<CouponResponse>>> GetAll()
    {
        var response = await _mediator.Send(new GetAllCouponQuery());
        return response;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CouponResponse>> Get(long id)
    {
        var response = await _mediator.Send(new GetCouponByIdQuery(id));
        return response;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CouponResponse>> Post(CouponRequest couponRequest)
    {
        var response = await _mediator.Send(new CreateCouponCommand(couponRequest));
        return response;
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse> Delete(long id)
    {
        var response = await _mediator.Send(new DeleteCouponCommand { Id = id });
        return response;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<CouponResponse>> Put(long id, [FromBody] CouponRequest couponRequest)
    {
        var response = await _mediator.Send(new UpdateCouponCommand(id, couponRequest));
        return response;
    }
}

