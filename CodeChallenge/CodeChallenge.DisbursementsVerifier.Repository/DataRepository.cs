using CodeChallenge.DisbursementsVerifier.Models;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class DataRepository : IDataRepository
{
    public IEnumerable<PayslipDetail> GetPayslipDetails()
    {
        return new List<PayslipDetail>()
        {
            new PayslipDetail()
            {
                Amount = 200,
                Code = "10 - Annual Lve",
                EmployeeCode = 1165,
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = DateTime.Now.AddMonths(1).AddDays(-1),
            }
        };
    }

    public IEnumerable<Disbursement> GetDisbursements()
    {
        var currentDate = DateTime.Now;

        return new List<Disbursement>()
        {
            new Disbursement()
            {
                Amount = 100,
                PaymentDate = currentDate,
                PeriodFromDate = new DateTime(currentDate.Year, currentDate.Month, 01),
                PeriodToDate = currentDate.AddMonths(1).AddDays(-1),
                EmployeeCode = 1165
            }
        };
    }

    public IEnumerable<PayCode> GetPayCodes()
    {
        return new List<PayCode>()
        {
            new PayCode()
            {
                Code = "10 - Annual Lve",
                OTETreatment = "OTE"
            },
            new PayCode()
            {
                Code = "1 - Normal",
                OTETreatment = "OTE"
            },
            new PayCode()
            {
                Code = "14 - LWOP",
                OTETreatment = "Not OTE"
            }
        };
    }

    public DisbursementSuperData GetDisbursementsSuperData()
    {
        var disbursements = GetDisbursements();
        var payCodes = GetPayCodes();
        var payslipDetails = GetPayslipDetails();

        return new DisbursementSuperData()
        {
            Disbursements = disbursements,
            PayCodes = payCodes,
            PayslipDetails = payslipDetails
        };
    }
}