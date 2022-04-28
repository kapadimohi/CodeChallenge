using System.Data;

namespace CodeChallenge.DisbursementsVerifier.Repository.Interfaces;

public interface IExcelDataAdapter
{
    DataSet GetData();
}