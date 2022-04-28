using System.Collections.Generic;
using CodeChallenge.DisbursementsVerifier.Api.Controllers;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CodeChallenge.DisbursementsVerifier.Api.UnitTests;

public class Tests
{
    [Fact]
    public void Given_WhenVerifyActionIsInvoked_ThenVerificationResultIsReturned()
    {
        var mockLogger = Mock.Of<ILogger<VerifyDisbursementsController>>();
        var mockDisbursementsVerifier = new Mock<IDisbursementsVerifier>();

        var expectedVerificationResult = new List<VerificationResult>();
        mockDisbursementsVerifier.Setup(m => m.Verify()).Returns(expectedVerificationResult);
            
        var controller = new VerifyDisbursementsController(mockDisbursementsVerifier.Object, mockLogger);
        var result = controller.Verify();

        mockDisbursementsVerifier.Verify(m => m.Verify(), Times.Once);

        Assert.Equal(expectedVerificationResult, result);
    }
}