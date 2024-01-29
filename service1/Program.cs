using Microsoft.EntityFrameworkCore;
using service1.Data;
using service1.Services;
using service1.Services.RabbitMQService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>
    (options => options.UseNpgsql(builder.Configuration.
        GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IMessageBusService, MessageBusService>();
builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var context = app.Services.GetRequiredService<AppDbContext>();

context.Database.Migrate();
SeedData.Initialize(context);
app.UseRouting();
app.MapControllers();
app.MapGrpcService<GrpcPostService>();
app.Run();
