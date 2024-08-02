using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.ProductCommands;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResponse<ProductResponse>> Post(ProductRequest couponRequest)
        {
            var response = await _mediator.Send(new CreateProductCommand(couponRequest));
            return response;
        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteProductCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<ProductResponse>> Put(long id, [FromBody] ProductRequest couponRequest)
        {
            var response = await _mediator.Send(new UpdateProductCommand(id, couponRequest));
            return response;
        }
    }
}
