using Application;
using Identity;
using Identity.Models;
using Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence;
using Shared;
using System;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationLayer();
builder.Services.AddIdentityInfraestructure(builder.Configuration);
builder.Services.AddSharedInfraestructure(builder.Configuration);
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseErrorHandlingMiddleware();

app.MapControllers();

try
{
    await SeedUsers();

    app.Run();

}
catch (Exception ex)
{

    throw;
}


async Task SeedUsers()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.SeedAsync(userManager, roleManager);
    await DefaultAdminUser.SeedAsync(userManager, roleManager);
    await DefaultBasicUser.SeedAsync(userManager, roleManager);
}
