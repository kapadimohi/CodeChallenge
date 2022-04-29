using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IDisbursementDataProcessor
{
    IEnumerable<ProcessedDisbursementData> AggregateByEmployeeAndPeriod(IEnumerable<Disbursement> disbursements);
}