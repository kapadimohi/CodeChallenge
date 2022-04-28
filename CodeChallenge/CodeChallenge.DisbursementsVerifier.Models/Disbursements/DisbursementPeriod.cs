namespace CodeChallenge.DisbursementsVerifier.Models.Disbursements;

public record DisbursementPeriod()
{
    public int Year { get; set; }
    public int Quarter { get; set; }
}