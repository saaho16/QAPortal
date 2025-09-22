using QAPortal.Data;
using QAPortal.Business;
using QAPortal.Business.Services;
// using QAPortal.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using QAPortal.Data.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExceptionFiltersDemo.Middlewares;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//adding Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

    };
});




// Adding Extensions
// Register dependencies using extension methods
builder.Services.AddBusinessLayer(builder.Configuration.GetConnectionString("DefaultConnection")!);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Adding Custom Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAngular");
app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
