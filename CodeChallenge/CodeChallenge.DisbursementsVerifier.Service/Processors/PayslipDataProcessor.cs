using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Extensions;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Service.Calculators;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Service.Processors;

public class PayslipDataProcessor : IPayslipDataProcessor
{
    private readonly ISuperCalculator _superCalculator;

    public PayslipDataProcessor(ISuperCalculator superCalculator)
    {
        _superCalculator = superCalculator;
    }

    public IEnumerable<ProcessedPayslipData> AggregateByEmployeeAndPeriod(IEnumerable<PayslipDetail> payslipDetails, IEnumerable<PayCode> payCodes)
    {
        var processedPayslipData = payslipDetails
            .GroupBy(p => new
            {
                p.EmployeeCode, 
                QuarterEndingDate = p.PayslipEndDate.GetQuarterEndingDate()
            })
            .Select(p => 
                new ProcessedPayslipData
                {
                    TotalOte = p.Where(p => payCodes.Any(pc => pc.OteTreatment == OteTreatment.OTE && p.Code == pc.Code)).Sum(l => l.Amount), 
                    EmployeeCode = p.Key.EmployeeCode, 
                    QuarterEndingDate = p.Key.QuarterEndingDate,
                    TotalSuperPayable = _superCalculator.CalculateSuperForGivenOTEAndPeriod(
                        p.Where(p => payCodes.Any(pc => pc.OteTreatment == OteTreatment.OTE.ToString() 
                                                        && p.Code == pc.Code))
                            .Sum(l => l.Amount),p.Key.QuarterEndingDate)
                })
            .OrderBy(p => p.EmployeeCode)
            .ToList();

        return processedPayslipData;
    }
}
