using System.Text;
using API;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices(builder.Configuration); 
builder.Services.AddIdentityService(builder.Configuration); 

// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseCors(a=>a.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:4200","https://localhost:4200")
            );

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();


app.MapControllers();

app.Run();
