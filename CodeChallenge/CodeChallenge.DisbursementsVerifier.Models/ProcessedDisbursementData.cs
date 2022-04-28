namespace CodeChallenge.DisbursementsVerifier.Models;

public class ProcessedDisbursementData
{
    public int EmployeeCode { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }
    public int Disbursement { get; set; }
}