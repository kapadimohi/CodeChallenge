using System.Data;
using System.Globalization;
using CodeChallenge.DisbursementsVerifier.Models;
using ExcelDataReader;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class DataRepository : IDataRepository
{
    public DisbursementSuperData GetDisbursementsSuperData()
    {
        var dataSet = RetrieveRawDataFromExcel();

        var disbursements = ParseDisbursements(dataSet.Tables[0]);
        var paySlipDetails = ParsePayslipDetails(dataSet.Tables[1]);
        var payCodes = ParsePayCodes(dataSet.Tables[2]);
        
        return new DisbursementSuperData()
        {
            Disbursements = disbursements,
            PayCodes = payCodes,
            PayslipDetails = paySlipDetails
        };
    }
    
    private IEnumerable<PayCode> ParsePayCodes(DataTable payCodesRawData)
    {
        var payCodes = new List<PayCode>();
        
        foreach (DataRow row in payCodesRawData.Rows)
        {
            var payCode = new PayCode()
            {
                Code = row["pay_code"].ToString(),
                OTETreatment = row["ote_treament"].ToString()
            };
            payCodes.Add(payCode);
        }

        return payCodes;
    }

    private IEnumerable<PayslipDetail> ParsePayslipDetails(DataTable payslipDetailsRawData)
    {
        var payslipDetails = new List<PayslipDetail>();
        
        foreach (DataRow row in payslipDetailsRawData.Rows)
        {
            var payslipDetail = new PayslipDetail()
            {
                PayslipId = Guid.Parse(row["payslip_id"].ToString()),
                PayslipEndDate = DateTime.Parse(row["end"].ToString(), CultureInfo.InvariantCulture),
                EmployeeCode = int.Parse(row["employee_code"].ToString(), CultureInfo.InvariantCulture),
                Code = row["code"].ToString(),
                Amount = Decimal.Parse(row["amount"].ToString()),
                
            };
            payslipDetails.Add(payslipDetail);
        }

        return payslipDetails;
    }

    private IEnumerable<Disbursement> ParseDisbursements(DataTable disbursementsRawData)
    {
        var disbursements = new List<Disbursement>();
        
        foreach (DataRow row in disbursementsRawData.Rows)
        {
            var disbursement = new Disbursement()
            {
                Amount = Decimal.Parse(row["sgc_amount"].ToString()),
                PaymentDate = DateTime.Parse(row["payment_made"].ToString(), CultureInfo.InvariantCulture),
                PeriodFromDate = DateTime.Parse(row["pay_period_from"].ToString(), CultureInfo.InvariantCulture),
                PeriodToDate = DateTime.Parse(row["pay_period_from"].ToString(), CultureInfo.InvariantCulture),
                EmployeeCode = int.Parse(row["employee_code"].ToString())
            };
            disbursements.Add(disbursement);
        }

        return disbursements;
    }
    
    private DataSet RetrieveRawDataFromExcel()
    {
        using (var stream = File.Open("SampleSuperData.xlsx", FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var config = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true 
                    }
                };
                
                var result = reader.AsDataSet(config);
                return result;
            }
        }
    }
}