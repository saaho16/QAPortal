using QAPortal.Business;
// using QAPortal.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExceptionFiltersDemo.Middlewares;
using QAPortal.Presentation.Middlewares;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddLogging(logging => logging.AddConsole());

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Adding Custom Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();



app.UseCors("AllowAngular");


app.UseAuthentication();

//Jwt Decoder
app.UseMiddleware<JwtUserMiddleware>();

app.UseAuthorization();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
