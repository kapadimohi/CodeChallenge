using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;

namespace CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

public interface IDataRepository
{
    Task<DisbursementSuperData> GetDisbursementsSuperData(string fileName);
}