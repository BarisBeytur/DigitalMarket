using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DigitalMarket.Business.CQRS.Commands.CategoryCommands;
using DigitalMarket.Business.Infrastructure.DependencyResolvers;
using DigitalMarket.Business.Infrastructure.Mapping.AutoMapper;
using DigitalMarket.Business.Validation;
using DigitalMarket.Data.Context;
using DigitalMarket.Data.UnitOfWork;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;


#region Services

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddControllers()
    .AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<BaseValidator>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacBusinessModule());
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