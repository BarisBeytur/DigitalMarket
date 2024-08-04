using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CategoryCommands;
using DigitalMarket.Business.CQRS.Queries.CategoryQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CategoryResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllCategoryQuery());
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<CategoryResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetCategoryByIdQuery(id));
            return response;
        }

        [HttpPost]
        public async Task<ApiResponse<CategoryResponse>> Post(CategoryRequest categoryRequest)
        {
            var response = await _mediator.Send(new CreateCategoryCommand(categoryRequest));
            return response;
        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteCategoryCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<CategoryResponse>> Put(long id, [FromBody] CategoryRequest categoryRequest)
        {
            var response = await _mediator.Send(new UpdateCategoryCommand(id, categoryRequest));
            return response;
        }
    }
}
