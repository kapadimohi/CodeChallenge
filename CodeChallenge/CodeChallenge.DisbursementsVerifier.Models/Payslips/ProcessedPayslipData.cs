using CodeChallenge.DisbursementsVerifier.Models.Extensions;

namespace CodeChallenge.DisbursementsVerifier.Models.Payslips;

public record ProcessedPayslipData
{
    public int EmployeeCode { get; set; }
    public DateTime QuarterEndingDate { get; set; }
    public int Year => QuarterEndingDate.Year;
    public int Quarter => QuarterEndingDate.GetQuarter();
    public decimal TotalOte { get; set; }
    public decimal TotalSuperPayable { get; set; }
}
