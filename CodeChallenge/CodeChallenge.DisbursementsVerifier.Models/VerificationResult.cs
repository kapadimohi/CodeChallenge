namespace CodeChallenge.DisbursementsVerifier.Models;

public record VerificationResult
{
    public int EmployeeCode { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }
    public decimal TotalOrdinaryTimeEarnings { get; set; }
    public decimal TotalSuperPayable { get; set; }
    public decimal TotalDisbursed { get; set; }
    public decimal Variance => TotalDisbursed - TotalSuperPayable;

}