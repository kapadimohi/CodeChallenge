using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.DisbursementsVerifier.Service;

public class DisbursementsVerifier : IDisbursementsVerifier
{
    private readonly ILogger<DisbursementsVerifier> _logger;
    private readonly IDataRepository _dataRepository;
    private readonly IPayslipDataProcessor _payslipDataProcessor;
    private readonly IDisbursementDataProcessor _disbursementDataProcessor;

    public DisbursementsVerifier(
        ILogger<DisbursementsVerifier> logger,
        IDataRepository dataRepository, 
        IPayslipDataProcessor payslipDataProcessor,
        IDisbursementDataProcessor disbursementDataProcessor)
    {
        _logger = logger;
        _dataRepository = dataRepository;
        _payslipDataProcessor = payslipDataProcessor;
        _disbursementDataProcessor = disbursementDataProcessor;
    }
    
    public async Task<IEnumerable<VerificationResult>> Verify(string fileName)
    {
        _logger.LogInformation("VerifyDisbursements service");
        
        var disbursementsSuperData = await _dataRepository.GetDisbursementsSuperData(fileName);

        var processedPayslipData =  _payslipDataProcessor.AggregateByEmployeeAndPeriod(disbursementsSuperData.PayslipDetails, disbursementsSuperData.PayCodes);

        var processedDisbursementData =  _disbursementDataProcessor.AggregateByEmployeeAndPeriod(disbursementsSuperData.Disbursements);

        return MergePayslipAndDisbursementData(processedPayslipData, processedDisbursementData);
    }
    
    private static IEnumerable<VerificationResult> MergePayslipAndDisbursementData(IEnumerable<ProcessedPayslipData> processedPayslipData,
        IEnumerable<ProcessedDisbursementData> processedDisbursementData)
    {
        // var query =
        //     from paySlipData in processedPayslipData
        //     join disbursementData in processedDisbursementData
        //         on new {paySlipData.EmployeeCode, paySlipData.Year, paySlipData.Quarter}
        //         equals new {disbursementData.EmployeeCode, disbursementData.Year, disbursementData.Quarter} into
        //         payslipAndDisbursement
        //     from disbursement in payslipAndDisbursement.DefaultIfEmpty()
        //     select new VerificationResult
        //     {
        //         EmployeeCode = paySlipData.EmployeeCode,
        //         Year = paySlipData.Year,
        //         Quarter = paySlipData.Quarter,
        //         TotalOrdinaryTimeEarnings = paySlipData.TotalOte,
        //         TotalSuperPayable = paySlipData.TotalSuperPayable,
        //         TotalDisbursed = disbursement?.Disbursement ?? 0
        //     };


        var processedPayslipDatas = processedPayslipData.ToList();
        
        var x = processedPayslipDatas.Select(p =>
            new
            {
                p.EmployeeCode,
                p.Year,
                p.Quarter
            });

        var processedDisbursementDatas = processedDisbursementData.ToList();
        
        var y = processedDisbursementDatas.Select(p =>
            new
            {
                p.EmployeeCode,
                p.Year,
                p.Quarter
            });

        var employeePeriods = x.Union(y).Distinct();

        var results = (from period in employeePeriods
        let disbursementData = processedDisbursementDatas.FirstOrDefault(d => d.EmployeeCode == period.EmployeeCode && d.Year == period.Year && d.Quarter == period.Quarter)
        let payslipData = processedPayslipDatas.FirstOrDefault(p => p.EmployeeCode == period.EmployeeCode && p.Year == period.Year && p.Quarter == period.Quarter)
        select new VerificationResult()
        {
            EmployeeCode = period.EmployeeCode,
            Year = period.Year,
            Quarter = period.Quarter,
            TotalDisbursed = disbursementData?.Disbursement ?? 0M,
            TotalOrdinaryTimeEarnings = payslipData?.TotalOte ?? 0M,
            TotalSuperPayable = payslipData?.TotalSuperPayable ?? 0M
        }).ToList();

        return results.OrderBy(q => q.EmployeeCode).ThenBy(q => q.Year).ThenBy(q => q.Quarter);
        
        //return query.OrderBy(q => q.EmployeeCode).ThenBy(q => q.Year).ThenBy(q => q.Quarter);
    }
}