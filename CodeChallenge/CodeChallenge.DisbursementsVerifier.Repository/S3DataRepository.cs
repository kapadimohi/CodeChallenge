using System.Data;
using Amazon.S3;
using Amazon.S3.Model;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class S3DataRepository : IDataRepository
{
    private readonly IAmazonS3 _s3Client;
    private readonly IExcelDataStreamAdapter _excelDataStreamAdapter;
    private readonly IDataParser _dataParser;
    private const string BucketName = "test";

    public S3DataRepository(IAmazonS3 s3Client,
        IExcelDataStreamAdapter excelDataStreamAdapter,
        IDataParser dataParser)
    {
        _s3Client = s3Client;
        _excelDataStreamAdapter = excelDataStreamAdapter;
        _dataParser = dataParser;
    }

    public async Task<DisbursementSuperData> GetDisbursementsSuperData(string fileName)
    {
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
        var response = await _s3Client.GetObjectAsync(new GetObjectRequest
        {
            Key = fileName,
            BucketName = bucketName,
        });
        
        var memoryStream = new MemoryStream();

        await using var responseStream = response.ResponseStream;
        await responseStream.CopyToAsync(memoryStream);

        return memoryStream;
    }
}