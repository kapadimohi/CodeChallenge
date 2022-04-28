using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IPayslipDataProcessor
{
    IEnumerable<ProcessedPayslipData> AggregateByEmployeeAndPeriod(IEnumerable<PayslipDetail> payslipDetails, IEnumerable<PayCode> payCodes);
}