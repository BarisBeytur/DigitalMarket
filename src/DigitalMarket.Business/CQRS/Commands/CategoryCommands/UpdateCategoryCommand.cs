using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.CQRS.Commands.CategoryCommands;

public class UpdateCategoryCommand : IRequest<ApiResponse<CategoryResponse>>
{
    public long Id { get; set; }
    public CategoryRequest Request { get; set; }

    public UpdateCategoryCommand(long id,CategoryRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse<CategoryResponse>>
{

    private readonly IUnitOfWork<Category> _unitOfWork;

    public UpdateCategoryCommandHandler(IUnitOfWork<Category> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<CategoryResponse>("Item not found");
        }

        // mapping
        item.Name = request.Request.Name;
        item.Url = request.Request.Url;
        item.Tags = request.Request.Tags;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<CategoryResponse>(
            new CategoryResponse
            {
                Id = item.Id,
                Name = item.Name,
                Url = item.Url,
                Tags = item.Tags
            });
    }
}
