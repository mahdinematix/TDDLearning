using Academy.Application;
using Academy.Domain;
using Academy.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<AcademyContext>(x =>
    x.UseSqlServer(
        "Data Source=.; Initial Catalog=TddAcademy; Persist Security Info=True; User ID=sa; Password=****; TrustServerCertificate=True;"));
services.AddTransient<ICourseRepository, CourseRepository>();
services.AddTransient<ICourseApplication, CourseApplication>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
