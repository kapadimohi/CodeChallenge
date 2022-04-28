using System.Data;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using ExcelDataReader;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class ExcelDataAdapter : IExcelDataAdapter
{
    private readonly ILogger<ExcelDataAdapter> _logger;
    private readonly  string _fileLocation = "SampleSuperData.xlsx";

    public ExcelDataAdapter(ILogger<ExcelDataAdapter> logger)
    {
        _logger = logger;
    }

    public DataSet GetData()
    {
        _logger.LogInformation($"Retrieving data from excel file {_fileLocation}");
        
        try
        {
            using (var stream = File.Open(_fileLocation, FileMode.Open, FileAccess.Read))
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
        catch (Exception e)
        {
            _logger.LogError($"Error reading data from file {_fileLocation}");
            throw;
        }
    }
}