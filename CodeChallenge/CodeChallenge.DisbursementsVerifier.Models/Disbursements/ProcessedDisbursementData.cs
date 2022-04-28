namespace CodeChallenge.DisbursementsVerifier.Models.Disbursements;

public class ProcessedDisbursementData
{
    public int EmployeeCode { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }
    public decimal Disbursement { get; set; }
}