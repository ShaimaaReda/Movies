using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using Core.IServices;
using Services;
using Core.IRepository;
using Repos.Repository;
using MovieServer.Hubs;
using BaseLibrary.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JWTsection"));
builder.Services.AddScoped(typeof(IGenericRepository<Movie>), typeof(GenericRepository<Movie>));
builder.Services.AddScoped(typeof(IGenericRepository<Review>), typeof(GenericRepository<Review>));


builder.Services.AddScoped<IUserAccount,UserAccount>();
//builder.Services.AddScoped<IGenericRepository<Movie>,GenericRepository<Movie>>();
//builder.Services.AddScoped<IGenericRepository<Review>,GenericRepository<Review>>();

builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("sorry your connection disconnected"));
});

var app = builder.Build();

app.UseRouting(); // Add routing middleware

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Map your Web API controllers
    endpoints.MapHub<ReviewHub>("/reviewHub"); // Map the SignalR hub
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
