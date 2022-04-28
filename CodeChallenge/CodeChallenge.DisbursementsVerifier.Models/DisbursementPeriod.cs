namespace CodeChallenge.DisbursementsVerifier.Models;

public record DisbursementPeriod()
{
    public int Year { get; set; }
    public int Quarter { get; set; }
}