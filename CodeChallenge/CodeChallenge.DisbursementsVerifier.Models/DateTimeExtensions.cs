namespace CodeChallenge.DisbursementsVerifier.Models;

public static class DateTimeExtensions
{
    /// <summary>
    /// Get the quarter for a given date e.g. 31st January is Quarter 1
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int GetQuarter(this DateTime date)
    {
        return (date.Month + 2)/3;
    }

    /// <summary>
    /// Get the quarter end date for a given date e.g. for 28th February, the quarter is 1 and quarter end date is 31st March
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime GetQuarterEndingDate(this DateTime date)
    {
        return date.Date.AddDays(1 - date.Day).AddMonths(3 - (date.Month - 1) % 3).AddDays(-1);
    }
    
    /// <summary>
    /// Get the quarter for a disbursement payment which is slightly different to a ordinary quarter
    /// Disbursements are allowed for up to 28 days after the end of a quarter
    /// e.g. A payment made on April 26th, should be attributed to quarter 1
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
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