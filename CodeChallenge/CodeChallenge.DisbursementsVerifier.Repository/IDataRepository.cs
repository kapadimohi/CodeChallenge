using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public interface IDataRepository
{
    IEnumerable<PayslipDetail> GetPayslipDetails();
    IEnumerable<Disbursement> GetDisbursements();
    IEnumerable<PayCode> GetPayCodes();
    DisbursementSuperData GetDisbursementsSuperData();
}