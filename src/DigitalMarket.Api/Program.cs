using DigitalMarket.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


#region Services

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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