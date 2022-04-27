namespace CodeChallenge.DisbursementsVerifier.Service.Models;

public record PayCode
{
    public string Code { get; set; }
    public string OTETreatment { get; set; }
}