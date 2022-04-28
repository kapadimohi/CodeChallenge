using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Service.Processors;

public class DisbursementDataProcessor : IDisbursementDataProcessor
{
    public IEnumerable<ProcessedDisbursementData> AggregteByEmployeeAndPeriod(IEnumerable<Disbursement> disbursements)
    {
        var processedDisbursementData = disbursements.GroupBy(d => new
            {
                d.EmployeeCode,
                Year = (d.PaymentDate.GetDisbursementPeriod().Year),
                Quarter = (d.PaymentDate.GetDisbursementPeriod().Quarter)
            })
            .Select(d =>
                new ProcessedDisbursementData
                {
                    EmployeeCode = d.Key.EmployeeCode,
                    Quarter = d.Key.Quarter,
                    Year = d.Key.Year,
                    Disbursement = d.Sum(ds => ds.Amount),
                })
            .OrderBy(d => d.EmployeeCode)
            .ToList();

        return processedDisbursementData;
    }
}