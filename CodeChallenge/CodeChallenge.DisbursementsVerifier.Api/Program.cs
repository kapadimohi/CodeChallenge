using CodeChallenge.DisbursementsVerifier.Repository;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using CodeChallenge.DisbursementsVerifier.Service;
using CodeChallenge.DisbursementsVerifier.Service.Calculators;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;
using CodeChallenge.DisbursementsVerifier.Service.Processors;

var builder = WebApplication.CreateBuilder(args);

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDisbursementsVerifier, DisbursementsVerifier>();
builder.Services.AddSingleton<IDisbursementDataProcessor, DisbursementDataProcessor>();
builder.Services.AddSingleton<IPayslipDataProcessor, PayslipDataProcessor>();
builder.Services.AddSingleton<ISuperCalculator, SuperCalculator>();
builder.Services.AddSingleton<IDataRepository, LocalDataRepository>();


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