using System.Data;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;

namespace CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

public interface IDataParser
{
    IEnumerable<Disbursement> ParseDisbursements(DataTable dataTable);
    IEnumerable<PayslipDetail> ParsePayslipDetails(DataTable dataTable);
    IEnumerable<PayCode> ParsePayCodes(DataTable dataTable);

}