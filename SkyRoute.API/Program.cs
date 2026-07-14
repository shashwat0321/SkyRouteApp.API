using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Interfaces;
using SkyRoute.Application.Interfaces.Repositories;
using SkyRoute.Application.Interfaces.Services;
using SkyRoute.Application.Mappers;
using SkyRoute.Application.Providers;
using SkyRoute.Application.Services;
using SkyRoute.Infrastructure.Data;
using SkyRoute.Infrastructure.Repositories;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<SkyRouteDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));
//builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();

builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IAirportService, AirportService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IFlightProvider, GlobalWingsProvider>();
builder.Services.AddScoped<IFlightProvider, BudgetWingsProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();