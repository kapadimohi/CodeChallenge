using CodeChallenge.DisbursementsVerifier.Models;
using ExcelDataReader;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class DataRepository : IDataRepository
{
    public IEnumerable<PayslipDetail> GetPayslipDetails()
    {
        return new List<PayslipDetail>()
        {
            new()
            {
                Amount = 2000,
                Code = "1 - Normal",
                EmployeeCode = 1165,
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 01,31),
            },
            new()
            {
                Amount = 10000,
                Code = "1 - Normal",
                EmployeeCode = 1165,
                PayslipId = Guid.NewGuid(),
                PayslipEndDate = new DateTime(2022, 05,30),
            }
        };
    }

    public IEnumerable<Disbursement> GetDisbursements()
    {
        return new List<Disbursement>()
        {
            new()
            {
                Amount = 200,
                PaymentDate = new DateTime(2022, 02,25),
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
        RetrieveRawDataFromExcel();
        
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


    private void RetrieveRawDataFromExcel()
    {
        using (var stream = File.Open("SampleSuperData.xlsx", FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                // The result of each spreadsheet is in result.Tables
            }
        }

    }
}