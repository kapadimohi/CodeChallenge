using System.Data;

namespace CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

public interface IExcelDataStreamAdapter
{
    Task<DataSet> GetData(Stream stream);
}