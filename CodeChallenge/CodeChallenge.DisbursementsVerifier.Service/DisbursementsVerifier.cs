using CodeChallenge.DisbursementsVerifier.Models;
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
    
    public IEnumerable<VerificationResult> Verify()
    {
        var disbursementsSuperData = _dataRepository.GetDisbursementsSuperData();

        var processedPayslipData = _payslipDataProcessor.Process(disbursementsSuperData.PayslipDetails, disbursementsSuperData.PayCodes);

        var processedDisbursementData = _disbursementDataProcessor.Process(disbursementsSuperData.Disbursements);

        return ProcessPayslipAndDisbursementData(processedPayslipData, processedDisbursementData);
    }

    private static IEnumerable<VerificationResult> ProcessPayslipAndDisbursementData(IEnumerable<ProcessedPayslipData> processedPayslipData,
        IEnumerable<ProcessedDisbursementData> processedDisbursementData)
    {
        var query =
            from paySlipData in processedPayslipData
            join disbursementData in processedDisbursementData
                on new {paySlipData.EmployeeCode, paySlipData.Year, paySlipData.Quarter}
                equals new {disbursementData.EmployeeCode, disbursementData.Year, disbursementData.Quarter} into
                payslipAndDisbursement
            from disbursement in payslipAndDisbursement.DefaultIfEmpty()
            select new VerificationResult
            {
                EmployeeCode = paySlipData.EmployeeCode,
                Year = paySlipData.Year,
                Quarter = paySlipData.Quarter,
                TotalOrdinaryTimeEarnings = paySlipData.TotalOTE,
                TotalSuperPayable = paySlipData.TotalSuperPayable,
                TotalDisbursed = disbursement?.Disbursement ?? 0
            };

        return query.ToList();
    }
}