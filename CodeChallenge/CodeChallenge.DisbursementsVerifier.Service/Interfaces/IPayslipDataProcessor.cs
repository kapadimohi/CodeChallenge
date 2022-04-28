using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IPayslipDataProcessor
{
    IEnumerable<ProcessedPayslipData> AggregateByEmployeeAndPeriod(IEnumerable<PayslipDetail> payslipDetails, IEnumerable<PayCode> payCodes);
}