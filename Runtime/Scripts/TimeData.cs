using UnityEngine;

namespace StardewTimeSystem
{
    /// <summary>
    /// ScriptableObject containing time configuration data.
    /// Allows designers to tweak time settings without modifying code.
    /// Create via: Create > Stardew > Time System > Time Data
    /// </summary>
    [CreateAssetMenu(fileName = "TimeData", menuName = "Stardew/Time System/Time Data")]
    public class TimeData : ScriptableObject
    {
        [Header("Time Settings")]
        [Tooltip("Duration of a full day in real-world seconds.")]
        public float DayDurationSeconds = 120f;
        
        [Tooltip("Number of hours in a day.")]
        public int HoursInDay = 24;
        
        [Tooltip("Number of minutes in an hour.")]
        public int MinutesInHour = 60;
        
        [Header("Day/Night Cycle")]
        [Tooltip("Time (in hours) when the day starts (sunrise).")]
        [Range(0, 23)]
        public int DayStartHour = 6;
        
        [Tooltip("Time (in hours) when the night starts (sunset).")]
        [Range(0, 23)]
        public int NightStartHour = 18;
        
        [Header("Time of Day Ranges")]
        [Tooltip("Hour when dawn starts (0-23).")]
        [Range(0, 23)]
        public int DawnStartHour = 0;
        
        [Tooltip("Hour when dawn ends (0-23).")]
        [Range(0, 23)]
        public int DawnEndHour = 6;
        
        [Tooltip("Hour when morning starts (0-23).")]
        [Range(0, 23)]
        public int MorningStartHour = 6;
        
        [Tooltip("Hour when morning ends (0-23).")]
        [Range(0, 23)]
        public int MorningEndHour = 12;
        
        [Tooltip("Hour when afternoon starts (0-23).")]
        [Range(0, 23)]
        public int AfternoonStartHour = 12;
        
        [Tooltip("Hour when afternoon ends (0-23).")]
        [Range(0, 23)]
        public int AfternoonEndHour = 17;
        
        [Tooltip("Hour when evening starts (0-23).")]
        [Range(0, 23)]
        public int EveningStartHour = 17;
        
        [Tooltip("Hour when evening ends (0-23).")]
        [Range(0, 23)]
        public int EveningEndHour = 20;
        
        [Header("Seasons")]
        [Tooltip("Number of days in each season.")]
        public int DaysInSeason = 28;
        
        [Tooltip("Number of seasons in a year.")]
        public int SeasonsInYear = 4;
        
        [Header("Time Scaling")]
        [Tooltip("Multiplier for time speed (1.0 = normal, 2.0 = 2x faster).")]
        public float TimeScale = 1.0f;
        
        [Tooltip("Whether time should pause when the game is not focused.")]
        public bool PauseWhenNotFocused = true;
        
        [Header("Start Time")]
        [Tooltip("Starting year.")]
        public int StartYear = 1;
        
        [Tooltip("Starting season (0 = Spring, 1 = Summer, 2 = Fall, 3 = Winter).")]
        [Range(0, 3)]
        public int StartSeason = 0;
        
        [Tooltip("Starting day of the season (1-28).")]
        [Range(1, 28)]
        public int StartDay = 1;
        
        [Tooltip("Starting hour of the day (0-23).")]
        [Range(0, 23)]
        public int StartHour = 6;
        
        [Tooltip("Starting minute of the hour (0-59).")]
        [Range(0, 59)]
        public int StartMinute = 0;
        
        [Header("Debug")]
        [Tooltip("Enable debug logging for time system events.")]
        public bool EnableDebugLogs = false;
        
        // Computed properties
        /// <summary>Gets the total number of minutes in a day.</summary>
        public int TotalMinutesInDay => HoursInDay * MinutesInHour;
        
        /// <summary>Gets the duration of one minute in real-world seconds.</summary>
        public float MinuteDurationSeconds => DayDurationSeconds / TotalMinutesInDay;
        
        /// <summary>Gets the duration of one hour in real-world seconds.</summary>
        public float HourDurationSeconds => MinuteDurationSeconds * MinutesInHour;
        
        /// <summary>Gets the total number of days in a year.</summary>
        public int TotalDaysInYear => DaysInSeason * SeasonsInYear;
        
        /// <summary>
        /// Gets the TimeOfDay for a given hour.
        /// </summary>
        /// <param name="hour">The hour to check (0-23)</param>
        /// <returns>The TimeOfDay for the given hour</returns>
        public TimeOfDay GetTimeOfDay(int hour)
        {
            if (hour >= DawnStartHour && hour < DawnEndHour) return TimeOfDay.Dawn;
            if (hour >= MorningStartHour && hour < MorningEndHour) return TimeOfDay.Morning;
            if (hour >= AfternoonStartHour && hour < AfternoonEndHour) return TimeOfDay.Afternoon;
            if (hour >= EveningStartHour && hour < EveningEndHour) return TimeOfDay.Evening;
            return TimeOfDay.Night;
        }
        
        /// <summary>
        /// Gets whether it's daytime for a given hour.
        /// </summary>
        /// <param name="hour">The hour to check (0-23)</param>
        /// <returns>True if it's daytime, false otherwise</returns>
        public bool IsDaytime(int hour)
        {
            return hour >= DayStartHour && hour < NightStartHour;
        }
    }
}
