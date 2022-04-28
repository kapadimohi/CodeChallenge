namespace CodeChallenge.DisbursementsVerifier.Models;

public static class DateTimeExtensions
{
    public static int GetQuarter(this DateTime date)
    {
        return (date.Month + 2)/3;
    }
    
    public static DisbursementPeriod GetDisbursementPeriod(this DateTime date)
    {
        date = date.AddDays(-28);
        var lastDayOfDisbursementQuarter = date.Date.AddDays(1 - date.Day).AddMonths(3 - (date.Month - 1) % 3).AddDays(-1).AddDays(28);
        var firstDayOfDisbursementQuarter = lastDayOfDisbursementQuarter.AddMonths(-3).AddDays(1);

        var period = new DisbursementPeriod
        {
            Year = firstDayOfDisbursementQuarter.Year,
            Quarter = firstDayOfDisbursementQuarter.GetQuarter()
        };
        return period;
    }
}