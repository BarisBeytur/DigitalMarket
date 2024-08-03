using AutoMapper;
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
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork<Category> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<CategoryResponse>("Item not found");

        _mapper.Map(request.Request, item);

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<CategoryResponse>(_mapper.Map<CategoryResponse>(item));
    }
}
