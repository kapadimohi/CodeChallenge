namespace CodeChallenge.DisbursementsVerifier.Models;

public record PayCode
{
    public string Code { get; set; }
    public string OteTreatment { get; set; }
}

public enum OteTreatment
{
    Ote,
    NotOte
}