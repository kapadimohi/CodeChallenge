using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
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
    
    public async Task<IEnumerable<VerificationResult>> Verify(string fileName)
    {
        var disbursementsSuperData = await _dataRepository.GetDisbursementsSuperData(fileName);

        var processedPayslipData =  _payslipDataProcessor.AggregateByEmployeeAndPeriod(disbursementsSuperData.PayslipDetails, disbursementsSuperData.PayCodes);

        var processedDisbursementData =  _disbursementDataProcessor.AggregteByEmployeeAndPeriod(disbursementsSuperData.Disbursements);

        return MergePayslipAndDisbursementData(processedPayslipData, processedDisbursementData);
    }
    
    private static IEnumerable<VerificationResult> MergePayslipAndDisbursementData(IEnumerable<ProcessedPayslipData> processedPayslipData,
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
                TotalOrdinaryTimeEarnings = paySlipData.TotalOte,
                TotalSuperPayable = paySlipData.TotalSuperPayable,
                TotalDisbursed = disbursement?.Disbursement ?? 0
            };

        return query.OrderBy(q => q.EmployeeCode).ThenBy(q => q.Year).ThenBy(q => q.Quarter);
    }
}