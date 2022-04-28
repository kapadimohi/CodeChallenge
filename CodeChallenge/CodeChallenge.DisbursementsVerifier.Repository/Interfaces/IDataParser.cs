using System.Data;
using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

public interface IDataParser
{
    IEnumerable<Disbursement> ParseDisbursements(DataTable dataTable);
    IEnumerable<PayslipDetail> ParsePayslipDetails(DataTable dataTable);
    IEnumerable<PayCode> ParsePayCodes(DataTable dataTable);

}