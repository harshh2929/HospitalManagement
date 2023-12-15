global using HospitalManagement.Models;
global using HospitalManagement.Data;
using HospitalManagement.Services.AdminService;
using HospitalManagement.Services.DoctorService;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In=ParameterLocation.Header,
        Name= "Authorization",
        Type=SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddDbContext<DataContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
