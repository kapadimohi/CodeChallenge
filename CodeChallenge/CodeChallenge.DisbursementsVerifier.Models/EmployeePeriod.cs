namespace CodeChallenge.DisbursementsVerifier.Models;

public record EmployeePeriod
{
    public int EmployeeCode { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }

}