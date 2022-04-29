using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CodeChallenge.DisbursementsVerifier.Repository.UnitTests;

public class DataParserTests
{
    [Fact]
    public void GivenDisbursementsDataTable_WhenParseIsInvoked_ThenAListOfDisbursementShouldBeReturned()
    {
        var mockLogger = Mock.Of<ILogger<DataParser>>();
        
        var disbursementDataTable = new DataTable();
        disbursementDataTable.Columns.Add("sgc_amount");
        disbursementDataTable.Columns.Add("payment_made");
        disbursementDataTable.Columns.Add("pay_period_from");
        disbursementDataTable.Columns.Add("pay_period_to");
        disbursementDataTable.Columns.Add("employee_code");

        var row = disbursementDataTable.NewRow();
        row[0] = 100.00;
        row[1] = "2017-08-09T00:00:00";
        row[2] = "2017-06-19T00:00:00";
        row[3] = "2017-07-30T00:00:00";
        row[4] = "50015418";
        
        disbursementDataTable.Rows.Add(row);
        
        var parser = new DataParser(mockLogger);
        var result = parser.ParseDisbursements(disbursementDataTable).ToList();

        Assert.Single(result);
        Assert.Equal(100.00M, result.First().Amount);
        Assert.Equal(50015418, result.First().EmployeeCode);
        Assert.Equal(new DateTime(2017,08,09).Date, result.First().PaymentDate.Date);
    }
    
    [Fact]
    public void GivenPayslipDetailsDataTable_WhenParseIsInvoked_ThenAListOfPayPayslipDetailsShouldBeReturned()
    {
        var mockLogger = Mock.Of<ILogger<DataParser>>();
        
        var payslipDetailsDataTables = new DataTable();
        payslipDetailsDataTables.Columns.Add("payslip_id");
        payslipDetailsDataTables.Columns.Add("end");
        payslipDetailsDataTables.Columns.Add("employee_code");
        payslipDetailsDataTables.Columns.Add("code");
        payslipDetailsDataTables.Columns.Add("amount");


        var row = payslipDetailsDataTables.NewRow();
        row[0] = "796553a0-0548-4357-b668-1871aee3ef9a";
        row[1] = "7/16/2017";
        row[2] = "50015418";
        row[3] = "1 - Normal";
        row[4] = "4038.47";
        
        payslipDetailsDataTables.Rows.Add(row);
        
        var parser = new DataParser(mockLogger);
        var result = parser.ParsePayslipDetails(payslipDetailsDataTables).ToList();

        Assert.Single(result);
        Assert.Equal("796553a0-0548-4357-b668-1871aee3ef9a", result.First().PayslipId.ToString());
        Assert.Equal(new DateTime(2017,7,16).Date, result.First().PayslipEndDate.Date);
        Assert.Equal(50015418, result.First().EmployeeCode);
        Assert.Equal("1 - Normal", result.First().Code);
        Assert.Equal(4038.47M, result.First().Amount);

    }
    
    [Fact]
    public void GivenPayCodesDataTable_WhenParseIsInvoked_ThenAListOfPayCodesShouldBeReturned()
    {
        var mockLogger = Mock.Of<ILogger<DataParser>>();
        
        var payCodesDataTable = new DataTable();
        payCodesDataTable.Columns.Add("pay_code");
        payCodesDataTable.Columns.Add("ote_treament");

        var row = payCodesDataTable.NewRow();
        row[0] = "10 - Annual Lve";
        row[1] = "OTE";
        
        payCodesDataTable.Rows.Add(row);
        
        var parser = new DataParser(mockLogger);
        var result = parser.ParsePayCodes(payCodesDataTable).ToList();

        Assert.Single(result);
        Assert.Equal("10 - Annual Lve", result.First().Code);
        Assert.Equal("OTE", result.First().OteTreatment);
    }
}