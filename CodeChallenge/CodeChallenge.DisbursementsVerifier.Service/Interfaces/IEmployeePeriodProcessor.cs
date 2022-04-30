using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IEmployeePeriodProcessor
{
    IEnumerable<EmployeePeriod> GetPeriods(IEnumerable<ProcessedPayslipData> payslipData,
        IEnumerable<ProcessedDisbursementData> disbursementData);
}