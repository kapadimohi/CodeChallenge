namespace CodeChallenge.DisbursementsVerifier.Models.Payslips;

public record PayslipDetail
{
    public Guid PayslipId { get; set; }
    public DateTime PayslipEndDate { get; set; }
    public int EmployeeCode { get; set; }
    public string? Code { get; set; }
    public decimal Amount { get; set; }
}