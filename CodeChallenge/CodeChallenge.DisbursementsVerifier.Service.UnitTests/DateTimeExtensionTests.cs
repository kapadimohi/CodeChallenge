using System;
using CodeChallenge.DisbursementsVerifier.Models;
using Xunit;

namespace CodeChallenge.DisbursementVerifier.Service.UnitTests;

public class DateTimeExtensionTests
{
    [Theory]
    [InlineData("2022-03-31", 1)]
    [InlineData("2022-04-01", 2)]
    [InlineData("2023-07-18", 3)]
    [InlineData("2022-12-31", 4)]
    public void GivenADate_WhenGetQuarterIsInvoked_ThenCorrectQuarterNeedsToBeReturned(string stringDate, int expectedQuarter)
    {
        var date = DateTime.Parse(stringDate);
        var quarter = date.GetQuarter();
        
        Assert.Equal(expectedQuarter, quarter);
    }
    
    [Theory]
    [InlineData("2022-04-28", 1, 2022)]
    [InlineData("2022-04-29", 2, 2022)]
    [InlineData("2023-01-01", 4, 2022)]
    [InlineData("2022-02-28", 1, 2022)]
    [InlineData("2022-07-29", 3, 2022)]
    public void GivenADisbursementDate_WhenGetDisbursementPeriodIsInvoked_ThenCorrectPeriodNeedsToBeReturned(string stringDate, int expectedQuarter, int expectedYear)
    {
        var date = DateTime.Parse(stringDate);
        var disbursementPeriod = date.GetDisbursementPeriod();
        
        Assert.Equal(expectedQuarter, disbursementPeriod.Quarter);
        Assert.Equal(expectedYear, disbursementPeriod.Year);
    }
}