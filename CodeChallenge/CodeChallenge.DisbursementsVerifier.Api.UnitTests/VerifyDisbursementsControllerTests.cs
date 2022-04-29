using System.Collections.Generic;
using CodeChallenge.DisbursementsVerifier.Api.Controllers;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CodeChallenge.DisbursementsVerifier.Api.UnitTests;

public class Tests
{
    [Fact]
    public async void Given_WhenVerifyActionIsInvoked_ThenVerificationResultIsReturned()
    {
        var mockLogger = Mock.Of<ILogger<VerifyDisbursementsController>>();
        var mockDisbursementsVerifier = new Mock<IDisbursementsVerifier>();

        var expectedVerificationResult = new List<VerificationResult>()
        {
            new()
            {
                Quarter = 1,
                Year = 2022,
                EmployeeCode = 111,
                TotalDisbursed = 100,
                TotalSuperPayable = 10,
                TotalOrdinaryTimeEarnings = 1000
            }
        };
        mockDisbursementsVerifier.Setup(m => m.Verify()).ReturnsAsync(expectedVerificationResult);
            
        var controller = new VerifyDisbursementsController(mockDisbursementsVerifier.Object, mockLogger);
        var response = await controller.Verify();
        
        var objectResult = Assert.IsType<OkObjectResult>(response);
        
        mockDisbursementsVerifier.Verify(m => m.Verify(), Times.Once);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.Equal(expectedVerificationResult, objectResult.Value);
    }
}