using DigitalMarket.Business.CQRS.Commands.CategoryCommands;
using DigitalMarket.Business.CQRS.Commands.CouponCommands;
using DigitalMarket.Data.Context;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.GenericRepository;
using DigitalMarket.Data.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


#region Services

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateCategoryCommandHandler).GetTypeInfo().Assembly));

builder.Services.AddDbContext<DigitalMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DigitalMarketDbConnection"));
});


#endregion




#region Middlewares

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion