namespace StardewTimeSystem
{
    /// <summary>
    /// Represents the current time of day.
    /// </summary>
    public enum TimeOfDay
    {
        /// <summary>Early morning (default: 0:00 - 5:59)</summary>
        Dawn,
        /// <summary>Morning (default: 6:00 - 11:59)</summary>
        Morning,
        /// <summary>Afternoon (default: 12:00 - 16:59)</summary>
        Afternoon,
        /// <summary>Evening (default: 17:00 - 19:59)</summary>
        Evening,
        /// <summary>Night (default: 20:00 - 23:59)</summary>
        Night
    }

    /// <summary>
    /// Extension methods for TimeOfDay enum.
    /// </summary>
    public static class TimeOfDayExtensions
    {
        /// <summary>
        /// Converts TimeOfDay to a display-friendly string.
        /// </summary>
        /// <param name="timeOfDay">Time of day to convert</param>
        /// <returns>Display string</returns>
        public static string ToDisplayString(this TimeOfDay timeOfDay)
        {
            return timeOfDay switch
            {
                TimeOfDay.Dawn => "Dawn",
                TimeOfDay.Morning => "Morning",
                TimeOfDay.Afternoon => "Afternoon",
                TimeOfDay.Evening => "Evening",
                TimeOfDay.Night => "Night",
                _ => timeOfDay.ToString()
            };
        }

        /// <summary>
        /// Gets the next TimeOfDay in the cycle.
        /// </summary>
        /// <param name="timeOfDay">Current time of day</param>
        /// <returns>Next time of day</returns>
        public static TimeOfDay Next(this TimeOfDay timeOfDay)
        {
            int next = ((int)timeOfDay + 1) % 5;
            return (TimeOfDay)next;
        }
    }
}
