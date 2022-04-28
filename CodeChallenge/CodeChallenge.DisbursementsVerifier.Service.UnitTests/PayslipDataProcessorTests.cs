using System;
using System.Collections.Generic;
using System.Linq;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service;
using CodeChallenge.DisbursementsVerifier.Service.Calculators;
using CodeChallenge.DisbursementsVerifier.Service.Processors;
using Xunit;

namespace CodeChallenge.DisbursementVerifier.Service.UnitTests;

public class PayslipDataProcessorTests
{
    [Fact]
    public void GivenAllPayslipDataIsOteApplicable_WhenPayslipDataProcessorIsInvoked_ThenDataIsProcessedIntroAppropriateFormat()
    {
        var payslipDetails = new List<PayslipDetail>()
        {
            new()
            {
                EmployeeCode = 111,
                Amount = 100,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 01, 31),
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 200,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 02, 28)
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 300,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 03, 31)
            }
        };

        var payCodes = new List<PayCode>()
        {
            new() {Code = "Applicable Code", OTETreatment = "OTE"},
            new() {Code = "Non-Applicable Code", OTETreatment = "Not OTE"}
        };
        
        var processor = new PayslipDataProcessor(new SuperCalculator());
        var processedPayslipData = processor.AggregateByEmployeeAndPeriod(payslipDetails, payCodes).ToList();
        
        Assert.Single(processedPayslipData);
        Assert.Equal(600, processedPayslipData[0].TotalOte);
        Assert.Equal(2022, processedPayslipData[0].Year);
        Assert.Equal(1, processedPayslipData[0].Quarter);
        Assert.Equal(60, processedPayslipData[0].TotalSuperPayable);
    }

    [Fact]
    public void GivenAllPayslipDataIsNotOteApplicable_WhenPayslipDataProcessorIsInvoked_ThenDataIsProcessedIntroAppropriateFormat()
    {
        var payslipDetails = new List<PayslipDetail>()
        {
            new()
            {
                EmployeeCode = 111,
                Amount = 100,
                Code = "Non-Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 01, 31),
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 200,
                Code = "Non-Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 02, 28)
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 300,
                Code = "Non-Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 03, 31)
            }
        };

        var payCodes = new List<PayCode>()
        {
            new() {Code = "Applicable Code", OTETreatment = "OTE"},
            new() {Code = "Non-Applicable Code", OTETreatment = "Not OTE"}
        };
        
        var processor = new PayslipDataProcessor(new SuperCalculator());
        var processedPayslipData = processor.AggregateByEmployeeAndPeriod(payslipDetails, payCodes).ToList();
        
        Assert.Single(processedPayslipData);
        Assert.Equal(0, processedPayslipData[0].TotalOte);
        Assert.Equal(2022, processedPayslipData[0].Year);
        Assert.Equal(1, processedPayslipData[0].Quarter);
        Assert.Equal(0, processedPayslipData[0].TotalSuperPayable);
    }
    
    [Fact]
    public void GivenPayslipDataOverMultipleQuarters_WhenPayslipDataProcessorIsInvoked_ThenDataIsProcessedIntroAppropriateFormat()
    {
        var payslipDetails = new List<PayslipDetail>()
        {
            new()
            {
                EmployeeCode = 111,
                Amount = 100,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 01, 31),
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 200,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 02, 28)
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 300,
                Code = "Non-Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 05, 31)
            },
            new()
            {
                EmployeeCode = 111,
                Amount = 300,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 07, 31)
            },
            new()
            {
                EmployeeCode = 222,
                Amount = 1000,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 07, 31)
            },
            new()
            {
                EmployeeCode = 222,
                Amount = 1000,
                Code = "Applicable Code",
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 09, 30)
            }
        };

        var payCodes = new List<PayCode>()
        {
            new() {Code = "Applicable Code", OTETreatment = "OTE"},
            new() {Code = "Non-Applicable Code", OTETreatment = "Not OTE"}
        };
        
        var processor = new PayslipDataProcessor(new SuperCalculator());
        var processedPayslipData = processor.AggregateByEmployeeAndPeriod(payslipDetails, payCodes).ToList();
        
        Assert.Equal(4, processedPayslipData.Count);
        
        Assert.Equal(300, processedPayslipData[0].TotalOte);
        Assert.Equal(2022, processedPayslipData[0].Year);
        Assert.Equal(1, processedPayslipData[0].Quarter);
        Assert.Equal(30, processedPayslipData[0].TotalSuperPayable);
        
        Assert.Equal(300, processedPayslipData[2].TotalOte);
        Assert.Equal(2022, processedPayslipData[2].Year);
        Assert.Equal(3, processedPayslipData[2].Quarter);
        Assert.Equal(31.5M, processedPayslipData[2].TotalSuperPayable);
        
        Assert.Equal(2000, processedPayslipData[3].TotalOte);
        Assert.Equal(2022, processedPayslipData[3].Year);
        Assert.Equal(3, processedPayslipData[3].Quarter);
        Assert.Equal(210, processedPayslipData[3].TotalSuperPayable);
    }
}