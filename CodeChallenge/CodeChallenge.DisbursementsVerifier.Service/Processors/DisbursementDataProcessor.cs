using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Extensions;
using CodeChallenge.DisbursementsVerifier.Service.Interfaces;

namespace CodeChallenge.DisbursementsVerifier.Service.Processors;

public class DisbursementDataProcessor : IDisbursementDataProcessor
{
    public IEnumerable<ProcessedDisbursementData> AggregateByEmployeeAndPeriod(IEnumerable<Disbursement> disbursements)
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