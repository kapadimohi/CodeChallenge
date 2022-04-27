namespace CodeChallenge.DisbursementsVerifier.Service.Calculators;

public interface ISuperCalculator
{
    decimal CalculateSuperForGivenOTEAndPeriod(decimal ote, DateTime period);
}