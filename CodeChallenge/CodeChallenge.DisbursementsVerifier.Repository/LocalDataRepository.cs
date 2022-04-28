using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class LocalDataRepository : IDataRepository
{
    private readonly IExcelDataAdapter _excelDataAdapter;
    private readonly IDataParser _dataParser;

    public LocalDataRepository(
        IExcelDataAdapter excelDataAdapter,
        IDataParser dataParser
        )
    {
        _excelDataAdapter = excelDataAdapter;
        _dataParser = dataParser;
    }

    public DisbursementSuperData GetDisbursementsSuperData()
    {
        var dataSet = _excelDataAdapter.GetData();

        var disbursements = _dataParser.ParseDisbursements(dataSet.Tables[0]);
        var paySlipDetails = _dataParser.ParsePayslipDetails(dataSet.Tables[1]);
        var payCodes = _dataParser.ParsePayCodes(dataSet.Tables[2]);

        return new DisbursementSuperData()
        {
            Disbursements = disbursements,
            PayCodes = payCodes,
            PayslipDetails = paySlipDetails
        };
    }

    
}