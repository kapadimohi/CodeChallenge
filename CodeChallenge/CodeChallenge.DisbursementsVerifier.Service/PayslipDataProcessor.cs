using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service.Calculators;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Service;

public class PayslipDataProcessor : IPayslipDataProcessor
{
    private readonly ISuperCalculator _superCalculator;

    public PayslipDataProcessor(ISuperCalculator superCalculator)
    {
        _superCalculator = superCalculator;
    }

    public IEnumerable<ProcessedPayslipData> Process(IEnumerable<PayslipDetail> payslipDetails, IEnumerable<PayCode> payCodes)
    {
        var processedPayslipData = payslipDetails
            
            .GroupBy(p => new
            {
                p.EmployeeCode, 
                QuarterEndingDate = (p.PayslipEndDate.Date.AddDays(1 - p.PayslipEndDate.Day).AddMonths(3 - (p.PayslipEndDate.Month - 1) % 3).AddDays(-1))
            })
            .Select(p => 
                new ProcessedPayslipData
                {
                    TotalOTE = p.Where(p => payCodes.Any(pc => pc.OTETreatment == "OTE" && p.Code == pc.Code)).Sum(l => l.Amount), 
                    EmployeeCode = p.Key.EmployeeCode, 
                    QuarterEndingDate = p.Key.QuarterEndingDate,
                    TotalSuperPayable = _superCalculator.CalculateSuperForGivenOTEAndPeriod(p.Where(p => payCodes.Any(pc => pc.OTETreatment == "OTE" && p.Code == pc.Code)).Sum(l => l.Amount),p.Key.QuarterEndingDate)
                })
            .OrderBy(p => p.EmployeeCode)
            .ToList();

        return processedPayslipData;
    }
}
