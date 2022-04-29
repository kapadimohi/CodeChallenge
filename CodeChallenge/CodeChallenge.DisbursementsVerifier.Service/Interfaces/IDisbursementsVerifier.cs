using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IDisbursementsVerifier
{
    Task<IEnumerable<VerificationResult>> Verify(string fileName);
}