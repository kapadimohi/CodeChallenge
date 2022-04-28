namespace CodeChallenge.DisbursementsVerifier.Models;

public class DisbursementSuperData
{
    public IEnumerable<Disbursement> Disbursements { get; set; }
    public IEnumerable<PayslipDetail> PayslipDetails { get; set; }
    public IEnumerable<PayCode> PayCodes { get; set; }
}