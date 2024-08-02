using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.ProductCategoryCommands;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResponse<ProductCategoryResponse>> Post(ProductCategoryRequest couponRequest)
        {
            var response = await _mediator.Send(new CreateProductCategoryCommand(couponRequest));
            return response;
        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteProductCategoryCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<ProductCategoryResponse>> Put(long id, [FromBody] ProductCategoryRequest couponRequest)
        {
            var response = await _mediator.Send(new UpdateProductCategoryCommand(id, couponRequest));
            return response;
        }
    }
}
