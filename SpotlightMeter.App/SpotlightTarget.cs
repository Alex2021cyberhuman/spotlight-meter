namespace SpotlightMeter.App;

public record SpotlightTarget(Guid Id, Guid CompanyId, string Name, bool Active, DateTime SpotlightPick, DateTime SpotlightRelease, TimeSpan TotalSpotlightTime, TimeSpan TotalOffTime);

public static class Extensions
{
    public static string ToMinSec(this TimeSpan timeSpan)
    {
        return $"{timeSpan.TotalMinutes:00}:{timeSpan.Seconds:00}";
    }
}