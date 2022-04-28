using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IPayslipDataProcessor
{
    IEnumerable<ProcessedPayslipData> Process(IEnumerable<PayslipDetail> payslipDetails, IEnumerable<PayCode> payCodes);
}