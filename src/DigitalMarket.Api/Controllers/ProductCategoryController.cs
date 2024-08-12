using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.ProductCategoryCommands;
using DigitalMarket.Business.CQRS.Queries.ProductCategoryQueries;
using DigitalMarket.Business.CQRS.Queries.ProductProductCategoryQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<IEnumerable<ProductCategoryResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllProductCategoryQuery());
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<ProductCategoryResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetProductCategoryByIdQuery(id));
            return response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductCategoryResponse>> Post(ProductCategoryRequest couponRequest)
        {
            var response = await _mediator.Send(new CreateProductCategoryCommand(couponRequest));
            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteProductCategoryCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductCategoryResponse>> Put(long id, [FromBody] ProductCategoryRequest couponRequest)
        {
            var response = await _mediator.Send(new UpdateProductCategoryCommand(id, couponRequest));
            return response;
        }
    }
}
