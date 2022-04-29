using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using Amazon.S3;
using Amazon.S3.Model;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using Moq;
using Xunit;

namespace CodeChallenge.DisbursementsVerifier.Repository.UnitTests;

public class S3DataRepositoryTests
{
    [Fact]
    public async void GivenS3DataRepository_WhenGetDisbursementsSuperDataIsInvoked_ThenObjectFromS3ShouldBeRequested()
    {
        var mockExcelDataAdapter = new Mock<IExcelDataStreamAdapter>();
        var mockDataParser = new Mock<IDataParser>();
        var mockS3Client = new Mock<IAmazonS3>();

        var mockStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));
        var expectedResponse = new GetObjectResponse()
        {
            ResponseStream = mockStream
        };
        
        var expectedDataSet = new DataSet();
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        
        mockExcelDataAdapter.Setup(m => m.GetData(It.IsAny<Stream>())).ReturnsAsync(expectedDataSet);
        mockS3Client.Setup(m => m.GetObjectAsync(It.IsAny<GetObjectRequest>(),It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);
        
        var s3DataRepository = new S3DataRepository(mockS3Client.Object, mockExcelDataAdapter.Object,mockDataParser.Object);
        await s3DataRepository.GetDisbursementsSuperData();
        
        mockS3Client.Verify(m => m.GetObjectAsync(It.IsAny<GetObjectRequest>(),It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async void GivenS3DataRepository_WhenGetDisbursementsSuperDataIsInvoked_ThenExcelFileFromS3ShouldBeParsedIntoDataSet()
    {
        var mockExcelDataAdapter = new Mock<IExcelDataStreamAdapter>();
        var mockDataParser = new Mock<IDataParser>();
        var mockS3Client = new Mock<IAmazonS3>();

        var mockStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));
        var expectedResponse = new GetObjectResponse()
        {
            ResponseStream = mockStream
        };
        
        var expectedDataSet = new DataSet();
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        
        mockExcelDataAdapter.Setup(m => m.GetData(It.IsAny<Stream>())).ReturnsAsync(expectedDataSet);
        mockS3Client.Setup(m => m.GetObjectAsync(It.IsAny<GetObjectRequest>(),It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);
        
        var s3DataRepository = new S3DataRepository(mockS3Client.Object, mockExcelDataAdapter.Object,mockDataParser.Object);
        await s3DataRepository.GetDisbursementsSuperData();
        
        mockExcelDataAdapter.Verify(m => m.GetData(It.IsAny<Stream>()), Times.Once);
    }
    
    [Fact]
    public async void GivenS3DataRepository_WhenGetDisbursementsSuperDataIsInvoked_Then_Disbursements_PayslipDetails_And_PayCodes_DataShouldBeParsed()
    {
        var mockExcelDataAdapter = new Mock<IExcelDataStreamAdapter>();
        var mockDataParser = new Mock<IDataParser>();
        var mockS3Client = new Mock<IAmazonS3>();

        var mockStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever"));
        var expectedResponse = new GetObjectResponse()
        {
            ResponseStream = mockStream
        };
        
        var expectedDataSet = new DataSet();
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        
        mockDataParser.Setup(m => m.ParseDisbursements(It.IsAny<DataTable>())).Returns(new List<Disbursement>());
        mockDataParser.Setup(m => m.ParsePayCodes(It.IsAny<DataTable>())).Returns(new List<PayCode>());
        mockDataParser.Setup(m => m.ParsePayslipDetails(It.IsAny<DataTable>())).Returns(new List<PayslipDetail>());

        
        mockExcelDataAdapter.Setup(m => m.GetData(It.IsAny<Stream>())).ReturnsAsync(expectedDataSet);
        mockS3Client.Setup(m => m.GetObjectAsync(It.IsAny<GetObjectRequest>(),It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);
        
        var s3DataRepository = new S3DataRepository(mockS3Client.Object, mockExcelDataAdapter.Object,mockDataParser.Object);
        await s3DataRepository.GetDisbursementsSuperData();
        
        mockDataParser.Verify(m => m.ParseDisbursements(It.IsAny<DataTable>()), Times.Once);
        mockDataParser.Verify(m => m.ParsePayCodes(It.IsAny<DataTable>()), Times.Once);
        mockDataParser.Verify(m => m.ParsePayslipDetails(It.IsAny<DataTable>()), Times.Once);
    }
}