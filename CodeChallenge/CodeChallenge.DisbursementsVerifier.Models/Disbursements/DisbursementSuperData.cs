using CodeChallenge.DisbursementsVerifier.Models.Payslips;

namespace CodeChallenge.DisbursementsVerifier.Models.Disbursements;

public class DisbursementSuperData
{
    public IEnumerable<Disbursements.Disbursement> Disbursements { get; set; }
    public IEnumerable<PayslipDetail> PayslipDetails { get; set; }
    public IEnumerable<PayCode> PayCodes { get; set; }
}