namespace AdminApp.Helpers;

public static class DateTimeHelper
{
    public static string FormatDateTime(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
            return "Never";

        TimeSpan timeDifference = DateTime.UtcNow - dateTime;

        if (timeDifference.TotalSeconds < 60)
            return "Less than a minute ago";

        if (timeDifference.TotalMinutes < 60)
            return $"{Math.Floor(timeDifference.TotalMinutes)} minute{(Math.Floor(timeDifference.TotalMinutes) > 1 ? "s" : "")} ago";

        if (timeDifference.TotalHours < 24)
            return $"{Math.Floor(timeDifference.TotalHours)} hour{(Math.Floor(timeDifference.TotalHours) > 1 ? "s" : "")} ago";

        if (timeDifference.TotalDays < 7)
            return $"{Math.Floor(timeDifference.TotalDays)} day{(Math.Floor(timeDifference.TotalDays) > 1 ? "s" : "")} ago";

        if (timeDifference.TotalDays < 30)
            return $"{Math.Floor(timeDifference.TotalDays / 7)} week{(Math.Floor(timeDifference.TotalDays / 7) > 1 ? "s" : "")} ago";

        if (timeDifference.TotalDays < 365)
            return $"{Math.Floor(timeDifference.TotalDays / 30)} month{(Math.Floor(timeDifference.TotalDays / 30) > 1 ? "s" : "")} ago";

        return $"{Math.Floor(timeDifference.TotalDays / 365)} year{(Math.Floor(timeDifference.TotalDays / 365) > 1 ? "s" : "")} ago";
    }

    public static DateTime ToLocalTime(DateTime utcTime, TimeSpan timeZoneOffset)
    {
        return utcTime.Add(timeZoneOffset);
    }
}
