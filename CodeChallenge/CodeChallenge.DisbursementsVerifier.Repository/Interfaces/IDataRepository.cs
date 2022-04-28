using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

public interface IDataRepository
{
    DisbursementSuperData GetDisbursementsSuperData();
}