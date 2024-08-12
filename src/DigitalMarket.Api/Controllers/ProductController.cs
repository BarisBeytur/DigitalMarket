using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.ProductCommands;
using DigitalMarket.Business.CQRS.Queries.ProductQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<IEnumerable<ProductResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllProductQuery());
            return response;
        }

        [HttpGet("GetByCategoryId")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<List<ProductWithCategoryResponse>>> GetByCategory(long categoryId)
        {
            var response = await _mediator.Send(new GetProductByCategoryQuery { CategoryId = categoryId });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<ProductResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetProductByIdQuery(id));
            return response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductResponse>> Post(ProductRequest couponRequest)
        {
            var response = await _mediator.Send(new CreateProductCommand(couponRequest));
            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteProductCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ProductResponse>> Put(long id, [FromBody] ProductRequest couponRequest)
        {
            var response = await _mediator.Send(new UpdateProductCommand(id, couponRequest));
            return response;
        }
    }
}
