using System.Data;
using System.Globalization;
using CodeChallenge.DisbursementsVerifier.Models;
using CodeChallenge.DisbursementsVerifier.Models.Disbursements;
using CodeChallenge.DisbursementsVerifier.Models.Payslips;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class DataParser : IDataParser
{
    private readonly ILogger<DataParser> _logger;

    public DataParser(ILogger<DataParser> logger)
    {
        _logger = logger;
    }

    public IEnumerable<Disbursement> ParseDisbursements(DataTable dataTable)
    {
        _logger.LogInformation("Parsing disbursements data");
        
        var disbursements = new List<Disbursement>();
        
        foreach (DataRow row in dataTable.Rows)
        {
            var disbursement = new Disbursement()
            {
                Amount = Decimal.Parse(row["sgc_amount"].ToString()),
                PaymentDate = DateTime.Parse(row["payment_made"].ToString(), CultureInfo.InvariantCulture),
                PeriodFromDate = DateTime.Parse(row["pay_period_from"].ToString(), CultureInfo.InvariantCulture),
                PeriodToDate = DateTime.Parse(row["pay_period_to"].ToString(), CultureInfo.InvariantCulture),
                EmployeeCode = int.Parse(row["employee_code"].ToString())
            };
            disbursements.Add(disbursement);
        }

        return disbursements;
    }

    public IEnumerable<PayslipDetail> ParsePayslipDetails(DataTable dataTable)
    {
        _logger.LogInformation("Parsing PaySlip data");
        
        var payslipDetails = new List<PayslipDetail>();
        
        foreach (DataRow row in dataTable.Rows)
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

    public IEnumerable<PayCode> ParsePayCodes(DataTable dataTable)
    {
        _logger.LogInformation("Parsing PayCodes data");
        
        var payCodes = new List<PayCode>();
        
        foreach (DataRow row in dataTable.Rows)
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
}