using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Service.Interfaces;

public interface IDisbursementsVerifier
{
    IEnumerable<VerificationResult> Verify();
}