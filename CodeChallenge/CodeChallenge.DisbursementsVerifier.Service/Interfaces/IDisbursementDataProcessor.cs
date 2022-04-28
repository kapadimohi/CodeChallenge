using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IDisbursementDataProcessor
{
    IEnumerable<ProcessedDisbursementData> Process(IEnumerable<Disbursement> disbursements);
}