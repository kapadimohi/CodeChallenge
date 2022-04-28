using CodeChallenge.DisbursementsVerifier.Repository;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Service;

public class DisbursementsVerifier : IDisbursementsVerifier
{
    private readonly IDataRepository _dataRepository;
    private readonly IPayslipDataProcessor _payslipDataProcessor;
    private readonly IDisbursementDataProcessor _disbursementDataProcessor;

    public DisbursementsVerifier(IDataRepository dataRepository, 
        IPayslipDataProcessor payslipDataProcessor,
        IDisbursementDataProcessor disbursementDataProcessor)
    {
        _dataRepository = dataRepository;
        _payslipDataProcessor = payslipDataProcessor;
        _disbursementDataProcessor = disbursementDataProcessor;
    }
    
    
    public void Verify()
    {
        var disbursementsSuperData = _dataRepository.GetDisbursementsSuperData();

        var processedPayslipData = _payslipDataProcessor.Process(disbursementsSuperData.PayslipDetails, disbursementsSuperData.PayCodes);

        var processedDisbursementData = _disbursementDataProcessor.Process(disbursementsSuperData.Disbursements);
    }
    
    
    /*
    public void VerifyDisbursements()
    {
        //get disbursements super data (basically the file in memory)
        
        //group payslips into quarters and ignore non OTE
        
        //group disbursements into quarters
        
        //join 2 together to get variance
    }
    */
}