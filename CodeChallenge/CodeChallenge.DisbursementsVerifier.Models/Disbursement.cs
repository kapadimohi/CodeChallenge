namespace CodeChallenge.DisbursementsVerifier.Models;

public class Disbursement
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime PeriodFromDate { get; set; }
    public DateTime PeriodToDate { get; set; }
    public int EmployeeCode { get; set; }
}