using System.Data;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class LocalDataRepository : IDataRepository
{
    private readonly IExcelDataStreamAdapter _excelDataStreamAdapter;
    private readonly IDataParser _dataParser;
    private readonly  string _fileLocation = "SampleSuperData.xlsx";
    
    public LocalDataRepository(
        IExcelDataStreamAdapter excelDataStreamAdapter,
        IDataParser dataParser
        )
    {
        _excelDataStreamAdapter = excelDataStreamAdapter;
        _dataParser = dataParser;
    }

    public async Task<DisbursementSuperData> GetDisbursementsSuperData()
    {
        DataSet dataSet;

        await using (var stream = File.Open(_fileLocation, FileMode.Open, FileAccess.Read))
        {
            dataSet = await _excelDataStreamAdapter.GetData(stream);
        }
        
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