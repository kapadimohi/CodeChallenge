using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
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
builder.Services.AddSingleton<IDataParser, DataParser>();
builder.Services.AddSingleton<IExcelDataStreamAdapter, ExcelDataStreamAdapter>();

var awsSettings = builder.Configuration.GetSection("AWS");
var awsLocalModeEnvVariable = awsSettings.GetValue<string>("LocalMode");
var awsLocalMode = !string.IsNullOrWhiteSpace(awsLocalModeEnvVariable) &&
                   bool.Parse(awsLocalModeEnvVariable);

AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);

if (awsLocalMode)
{
    var awsKey = awsSettings.GetValue<string>("AccessKey");
    var awsSecret = awsSettings.GetValue<string>("SecretKey");
    var awsRegion = awsSettings.GetValue<string>("Region");
    var awsUrl = awsSettings.GetValue<string>("ServiceUrl");
    builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(awsKey, awsSecret,
        new AmazonS3Config()
        {
            ServiceURL = awsUrl,
            AuthenticationRegion = awsRegion,
            ForcePathStyle = true,
        }));
}
else
{
    builder.Services.AddAWSService<IAmazonS3>();
}

builder.Services.AddSingleton<IDataRepository, S3DataRepository>();

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