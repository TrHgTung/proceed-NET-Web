using Microsoft.EntityFrameworkCore;
using webapp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var allowCors = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowCors,
                      builder =>
                      {
                          builder
                                .AllowAnyOrigin()
                                // .WithOrigins("http://localhost:5500", "http://127.0.0.1:5500", "http://127.0.0.1:3000", "http://127.0.0.1:3000")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                // .AllowCredentials()
                                ;
                      });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(6);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowCors);

app.UseHttpsRedirection();

app.UseSession();

app.UseRouting();

app.MapControllers();

app.Run();