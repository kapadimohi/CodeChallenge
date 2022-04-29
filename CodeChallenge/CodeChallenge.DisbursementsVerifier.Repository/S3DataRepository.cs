using System.Data;
using Amazon.S3;
using Amazon.S3.Model;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class S3DataRepository : IDataRepository
{
    private readonly ILogger<S3DataRepository> _logger;
    private readonly IAmazonS3 _s3Client;
    private readonly IExcelDataStreamAdapter _excelDataStreamAdapter;
    private readonly IDataParser _dataParser;
    private const string BucketName = "test";

    public S3DataRepository(
        ILogger<S3DataRepository> logger,
        IAmazonS3 s3Client,
        IExcelDataStreamAdapter excelDataStreamAdapter,
        IDataParser dataParser)
    {
        _logger = logger;
        _s3Client = s3Client;
        _excelDataStreamAdapter = excelDataStreamAdapter;
        _dataParser = dataParser;
    }

    public async Task<DisbursementSuperData> GetDisbursementsSuperData(string fileName)
    {
        _logger.LogInformation("S3 data repository");
        
        DataSet dataSet;
        await using(var stream = await GetObjectStream(BucketName, fileName))
        {
            dataSet = await _excelDataStreamAdapter.GetData(stream);
        }
        
        var disbursements = _dataParser.ParseDisbursements(dataSet.Tables[0]);
        var paySlipDetails = _dataParser.ParsePayslipDetails(dataSet.Tables[1]);
        var payCodes = _dataParser.ParsePayCodes(dataSet.Tables[2]);

        return new DisbursementSuperData()
        {
            Disbursements = disbursements,
            PayCodes = payCodes,
            PayslipDetails = paySlipDetails
        };
    }

    private async Task<Stream> GetObjectStream(string bucketName, string fileName)
    {
        _logger.LogInformation("Calling S3 service for bucket {@bucketName} and file {@fileName}", bucketName, fileName);

        var response = await _s3Client.GetObjectAsync(new GetObjectRequest
        {
            Key = fileName,
            BucketName = bucketName,
        });
        
        _logger.LogInformation("Received response from S3");

        var memoryStream = new MemoryStream();

        await using var responseStream = response.ResponseStream;
        await responseStream.CopyToAsync(memoryStream);

        return memoryStream;
    }
}