using System;
using System.Collections.Generic;
using System.Linq;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Repository;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;
using Moq;
using Xunit;

namespace CodeChallenge.DisbursementVerifier.Service.UnitTests;

public class DisbursementsVerifierTests
{
    [Fact]
    public async void GivenValidPayslipAndDisbursementData_WhenVerifyIsInvoked_ThenVerificationResultsMustBeReturned()
    {
        var mockDataRepository = new Mock<IDataRepository>();
        var mockPayslipDataProcessor = new Mock<IPayslipDataProcessor>();
        var mockDisbursementsDataProcessor = new Mock<IDisbursementDataProcessor>();

        var stubPayslipDetails = new List<PayslipDetail>();

        var stubDisbursementsData = new List<Disbursement>();

        var stubPayCodes = new List<PayCode>()
        {
            new()
            {
                Code = "Code",
                OTETreatment = "OTE"
            }
        };

        var stubDisbursementSuperData = new DisbursementSuperData
        {
            PayslipDetails = stubPayslipDetails,
            Disbursements = stubDisbursementsData,
            PayCodes = stubPayCodes
        };

        var stubProcessedPaySlipData = new List<ProcessedPayslipData>()
        {
            new()
            {
                EmployeeCode = 1111,
                QuarterEndingDate = new DateTime(2022, 03,31),
                TotalSuperPayable = 100,
                TotalOte = 1000
            }
        };
        
        var stubProcessedDisbursementData = new List<ProcessedDisbursementData>()
        {
            new()
            {
                EmployeeCode = 1111,
                Disbursement = 100,
                Quarter = 1,
                Year = 2022
            }
        };

        var expectedResult = new VerificationResult()
        {
            Quarter = 1,
            Year = 2022,
            EmployeeCode = 1111,
            TotalDisbursed = 100,
            TotalOrdinaryTimeEarnings = 1000,
            TotalSuperPayable = 100,
        };

        mockPayslipDataProcessor.Setup(m =>
                m.AggregateByEmployeeAndPeriod(It.IsAny<IEnumerable<PayslipDetail>>(), It.IsAny<IEnumerable<PayCode>>()))
            .Returns(stubProcessedPaySlipData);
        
        mockDisbursementsDataProcessor.Setup(m =>
                m.AggregteByEmployeeAndPeriod(It.IsAny<IEnumerable<Disbursement>>()))
            .Returns(stubProcessedDisbursementData);
        
        mockDataRepository.Setup(m => m.GetDisbursementsSuperData()).ReturnsAsync(stubDisbursementSuperData);
        
        var verifier = new DisbursementsVerifier.Service.DisbursementsVerifier(mockDataRepository.Object,
            mockPayslipDataProcessor.Object,
            mockDisbursementsDataProcessor.Object);

        var results = await verifier.Verify();

        Assert.Single(results);
        Assert.Equal(expectedResult, results.First());
    }
}