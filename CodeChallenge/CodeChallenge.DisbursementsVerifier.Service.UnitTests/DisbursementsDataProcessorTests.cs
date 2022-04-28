using System;
using System.Collections.Generic;
using System.Linq;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service;
using CodeChallenge.DisbursementsVerifier.Service.Processors;
using Xunit;

namespace CodeChallenge.DisbursementVerifier.Service.UnitTests;

public class DisbursementsDataProcessorTests
{
    [Fact]
    public void GivenDisbursementsData_WhenDisbursementsDataProcessorIsInvoked_ThenDataIsProcessedIntroAppropriateFormat()
    {
        var disbursements = new List<Disbursement>()
        {
            new Disbursement()
            {
                EmployeeCode = 111,
                PaymentDate = new DateTime(2022, 04, 28),
                Amount = 1000
            },
            new Disbursement()
            {
                EmployeeCode = 111,
                PaymentDate = new DateTime(2022, 01, 29),
                Amount = 1000
            }
        };

        var processor = new DisbursementDataProcessor();
        var processedDisbursementData = processor.AggregteByEmployeeAndPeriod(disbursements).ToList();
        
        Assert.Single(processedDisbursementData);
        Assert.Equal(2000, processedDisbursementData[0].Disbursement);
        Assert.Equal(2022, processedDisbursementData[0].Year);
        Assert.Equal(1, processedDisbursementData[0].Quarter);

    }
}