using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public interface IDataRepository
{
    DisbursementSuperData GetDisbursementsSuperData();
}