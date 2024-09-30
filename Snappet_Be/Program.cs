using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Snappet_Be.Data;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(optiom =>
{
    optiom.AddPolicy("Snappet", build => build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<SWD392_SNAPPET_DBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddApplicationInsightsTelemetry();


//builder.Services.AddDbContext<SWD392_SNAPPET_DBContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Snappet");
app.UseAuthorization();

app.MapControllers();

app.Run();
