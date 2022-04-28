namespace CodeChallenge.DisbursementsVerifier.Service.Calculators;

public class SuperCalculator : ISuperCalculator
{
    public decimal CalculateSuperForGivenOTEAndPeriod(decimal ote, DateTime period)
    {
        var superRate = GetSuperRateForGivenPeriod(period);
        return ote * (superRate / 100);
    }

    /// <summary>
    /// Actual Super rates for reference
    /// 1 July 2002 – 30 June 2013	9.00%
    /// 1 July 2013 – 30 June 2014	9.25%
    /// 1 July 2014 – 30 June 2021	9.50%
    /// 1 July 2021 – 30 June 2022	10.00%
    /// 1 July 2022 – 30 June 2023	10.50%
    /// 1 July 2023 – 30 June 2024	11.00%
    /// 1 July 2024 – 30 June 2025	11.50%
    /// 1 July 2025 – 30 June 2026 and onwards	12.00%
    /// </summary>
    /// <param name="period"></param>
    private decimal GetSuperRateForGivenPeriod(DateTime period)
    {
        if (period >= new DateTime(2025,07,01))
            return 12.00M;
        if (period >= new DateTime(2024,07,01))
            return 11.50M;
        if (period >= new DateTime(2023,07,01))
            return 11.00M;
        if (period >= new DateTime(2022,07,01))
            return 10.50M;
        if (period >= new DateTime(2021,07,01))
            return 10.00M;
        if (period >= new DateTime(2014,07,01))
            return 9.50M;
        if (period >= new DateTime(2013,07,01))
            return 9.25M;
        if (period >= new DateTime(2002, 07, 01))
            return 9.00M;
        return 0.00M; //super was not applicable prior to July 2002
    }
}