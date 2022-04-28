using System;
using CodeChallenge.DisbursementsVerifier.Service.Calculators;
using Xunit;

namespace CodeChallenge.DisbursementVerifier.Service.UnitTests;

public class SuperCalculatorTests
{
    [Theory]
    [InlineData(100, "2030-08-30", 12)]
    [InlineData(100, "2025-06-29", 11.50)]
    [InlineData(100, "2024-11-01", 11.50)]
    [InlineData(100, "2024-01-01", 11.00)]
    [InlineData(100, "2023-07-01", 11.00)]
    [InlineData(100, "2023-04-01", 10.50)]
    [InlineData(100, "2022-09-01", 10.50)]
    [InlineData(100, "2022-06-30", 10.00)]
    [InlineData(100, "2021-07-01", 10.00)]
    [InlineData(100, "2021-05-31", 9.50)]
    [InlineData(100, "2014-07-01", 9.50)]
    [InlineData(100, "2013-12-31", 9.25)]
    [InlineData(100, "2005-12-31", 9.00)]
    [InlineData(100, "2000-01-01", 0.00)]
    [InlineData(0, "2024-11-01", 0.00)]
    public void GivenADate_WhenCalculateSuperForGivenOTEAndDateIsInvoked_ThenValidSuperValuesAreCalculated(decimal ote, string periodAsString, decimal expectedSuper)
    {
        var period = DateTime.Parse(periodAsString);
        
        var superCalculator = new SuperCalculator();
        var result = superCalculator.CalculateSuperForGivenOTEAndPeriod(ote, period);
        
        Assert.Equal(expectedSuper, result);
    }
}