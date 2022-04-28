using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

class DisbursementDataProcessor : IDisbursementDataProcessor
{
    public IEnumerable<ProcessedDisbursementData> Process(IEnumerable<Disbursement> disbursements)
    {
        throw new NotImplementedException();
    }
}