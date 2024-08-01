using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CategoryCommands;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
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
