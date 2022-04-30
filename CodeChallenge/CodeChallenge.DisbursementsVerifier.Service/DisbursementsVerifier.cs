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
    private readonly IEmployeePeriodProcessor _employeePeriodProcessor;

    public DisbursementsVerifier(
        ILogger<DisbursementsVerifier> logger,
        IDataRepository dataRepository, 
        IPayslipDataProcessor payslipDataProcessor,
        IDisbursementDataProcessor disbursementDataProcessor,
        IEmployeePeriodProcessor employeePeriodProcessor)
    {
        _logger = logger;
        _dataRepository = dataRepository;
        _payslipDataProcessor = payslipDataProcessor;
        _disbursementDataProcessor = disbursementDataProcessor;
        _employeePeriodProcessor = employeePeriodProcessor;
    }
    
    public async Task<IEnumerable<VerificationResult>> Verify(string fileName)
    {
        _logger.LogInformation("VerifyDisbursements service");
        
        var disbursementsSuperData = await _dataRepository.GetDisbursementsSuperData(fileName);

        var processedPayslipData =  _payslipDataProcessor.AggregateByEmployeeAndPeriod(disbursementsSuperData.PayslipDetails, disbursementsSuperData.PayCodes);

        var processedDisbursementData =  _disbursementDataProcessor.AggregateByEmployeeAndPeriod(disbursementsSuperData.Disbursements);

        return MergePayslipAndDisbursementData(processedPayslipData.ToList(), processedDisbursementData.ToList());
    }
    
    private IEnumerable<VerificationResult> MergePayslipAndDisbursementData(IList<ProcessedPayslipData> processedPayslipData,
        IList<ProcessedDisbursementData> processedDisbursementData)
    {
        var employeePeriods = _employeePeriodProcessor.GetPeriods(processedPayslipData, processedDisbursementData);
            
        var results = (from period in employeePeriods
        let disbursementData = processedDisbursementData.FirstOrDefault(d => d.EmployeeCode == period.EmployeeCode && d.Year == period.Year && d.Quarter == period.Quarter)
        let payslipData = processedPayslipData.FirstOrDefault(p => p.EmployeeCode == period.EmployeeCode && p.Year == period.Year && p.Quarter == period.Quarter)
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
    }
}