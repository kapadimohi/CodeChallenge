namespace CodeChallenge.DisbursementsVerifier.Models;

public record PayCode
{
    public string Code { get; set; }
    public string OteTreatment { get; set; }
}

public static class OteTreatment
{
    public const string OTE = "OTE";
    public const string NotOTE = "Not OTE";
}