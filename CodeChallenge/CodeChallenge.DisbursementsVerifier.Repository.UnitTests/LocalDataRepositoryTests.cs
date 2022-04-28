using System.Collections.Generic;
using System.Data;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using Moq;
using Xunit;

namespace CodeChallenge.DisbursementsVerifier.Repository.UnitTests;

public class LocalDataRepositoryTests
{
    [Fact]
    public void GivenLocalDataRepository_WhenGetDisbursementsSuperDataIsInvoked_ThenExcelDataShouldBeRetrieved()
    {
        var mockExcelDataAdapter = new Mock<IExcelDataAdapter>();
        var mockDataParser = new Mock<IDataParser>();

        var expectedDataSet = new DataSet();
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        
        mockExcelDataAdapter.Setup(m => m.GetData()).Returns(expectedDataSet);
        
        var localDataRepository = new LocalDataRepository(mockExcelDataAdapter.Object, mockDataParser.Object);
        localDataRepository.GetDisbursementsSuperData();
        
        mockExcelDataAdapter.Verify(m => m.GetData(), Times.Once);
    }
    
    [Fact]
    public void GivenLocalDataRepository_WhenGetDisbursementsSuperDataIsInvoked_Then_Disbursements_PayslipDetails_And_PayCodes_DataShouldBeParsed()
    {
        var mockExcelDataAdapter = new Mock<IExcelDataAdapter>();
        var mockDataParser = new Mock<IDataParser>();

        var expectedDataSet = new DataSet();
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        expectedDataSet.Tables.Add(new DataTable());
        
        mockExcelDataAdapter.Setup(m => m.GetData()).Returns(expectedDataSet);
        mockDataParser.Setup(m => m.ParseDisbursements(It.IsAny<DataTable>())).Returns(new List<Disbursement>());
        mockDataParser.Setup(m => m.ParsePayCodes(It.IsAny<DataTable>())).Returns(new List<PayCode>());
        mockDataParser.Setup(m => m.ParsePayslipDetails(It.IsAny<DataTable>())).Returns(new List<PayslipDetail>());

        var localDataRepository = new LocalDataRepository(mockExcelDataAdapter.Object, mockDataParser.Object);
        localDataRepository.GetDisbursementsSuperData();
        
        mockDataParser.Verify(m => m.ParseDisbursements(It.IsAny<DataTable>()), Times.Once);
        mockDataParser.Verify(m => m.ParsePayCodes(It.IsAny<DataTable>()), Times.Once);
        mockDataParser.Verify(m => m.ParsePayslipDetails(It.IsAny<DataTable>()), Times.Once);
    }
}