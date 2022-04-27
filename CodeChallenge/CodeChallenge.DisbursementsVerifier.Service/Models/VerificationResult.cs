namespace CodeChallenge.DisbursementsVerifier.Service.Models;

public record VerificationResult
{
    public decimal TotalOTE { get; set; }
    public decimal TotalSuperPayable { get; set; }
    public decimal TotalDisbursed { get; set; }
    public decimal Variance { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }
}