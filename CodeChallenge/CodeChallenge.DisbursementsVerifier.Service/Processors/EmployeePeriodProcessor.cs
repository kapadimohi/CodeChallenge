using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Service.Processors;

public class EmployeePeriodProcessor : IEmployeePeriodProcessor
{
    public IEnumerable<EmployeePeriod> GetPeriods(IEnumerable<ProcessedPayslipData> payslipData, IEnumerable<ProcessedDisbursementData> disbursementData)
    {
        var periodsAvailableInPayslipData = payslipData.Select(p =>
            new EmployeePeriod()
            {
                EmployeeCode = p.EmployeeCode,
                Year = p.Year,
                Quarter = p.Quarter
            });

        var periodsAvailableInDisbursementData = disbursementData.Select(p =>
            new EmployeePeriod()
            {
                EmployeeCode = p.EmployeeCode,
                Year = p.Year,
                Quarter = p.Quarter
            });

        return periodsAvailableInPayslipData.Union(periodsAvailableInDisbursementData).Distinct();
    }
}