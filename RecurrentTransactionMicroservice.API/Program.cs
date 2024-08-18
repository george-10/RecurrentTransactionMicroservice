using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using RecurrentTransactionMicroservice.Core.AutoMappers;
using RecurrentTransactionMicroservice.Core.Services;
using RecurrentTransactionMicroservice.Core.Services.gRPC;
using RecurrentTransactionMicroservice.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(RecurrentTransactionProfile));
// Register your DbContext with the necessary database provider
builder.Services.AddDbContext<RecurrentTransactionsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost:5432;Database=recurrent-transactions-db\n;Username=username;Password=mysecretpassword")));

// Add gRPC services
builder.Services.AddGrpc();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services.AddControllers();

// Add background service
builder.Services.AddHostedService<RecurringTransactionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map your gRPC services
 
app.UseGrpcWeb();
app.MapGrpcService<ServiceImpl>().EnableGrpcWeb();

app.Run();