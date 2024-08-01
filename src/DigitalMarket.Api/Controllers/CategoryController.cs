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
        public async Task<ApiResponse<CategoryResponse>> CreateCategory(CategoryRequest categoryRequest)
        {
            var command = new CreateCategoryCommand(categoryRequest);
            var response = await _mediator.Send(command);
            return response;
        }
    }
}
