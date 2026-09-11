using UnityEngine.Events;

namespace StardewTimeSystem
{
    /// <summary>
    /// Static event bus for time system events.
    /// Use this to subscribe to time-related events without direct dependencies.
    /// Example: TimeEvents.OnHourPassed.AddListener(MyHourlyMethod);
    /// </summary>
    public static class TimeEvents
    {
        // Minute events
        /// <summary>Invoked every in-game minute.</summary>
        public static readonly UnityEvent OnMinutePassed = new UnityEvent();
        
        // Hour events
        /// <summary>Invoked every in-game hour.</summary>
        public static readonly UnityEvent OnHourPassed = new UnityEvent();
        
        // Day events
        /// <summary>Invoked when a new day starts.</summary>
        public static readonly UnityEvent OnDayStarted = new UnityEvent();
        
        /// <summary>Invoked when a day ends.</summary>
        public static readonly UnityEvent OnDayEnded = new UnityEvent();
        
        // Night events
        /// <summary>Invoked when night starts.</summary>
        public static readonly UnityEvent OnNightStarted = new UnityEvent();
        
        /// <summary>Invoked when night ends (day starts).</summary>
        public static readonly UnityEvent OnNightEnded = new UnityEvent();
        
        // Season events
        /// <summary>Invoked when the season changes.</summary>
        public static readonly UnityEvent OnSeasonChanged = new UnityEvent();
        
        /// <summary>Invoked when spring starts.</summary>
        public static readonly UnityEvent OnSpringStarted = new UnityEvent();
        
        /// <summary>Invoked when summer starts.</summary>
        public static readonly UnityEvent OnSummerStarted = new UnityEvent();
        
        /// <summary>Invoked when fall starts.</summary>
        public static readonly UnityEvent OnFallStarted = new UnityEvent();
        
        /// <summary>Invoked when winter starts.</summary>
        public static readonly UnityEvent OnWinterStarted = new UnityEvent();
        
        // Year events
        /// <summary>Invoked when the year changes.</summary>
        public static readonly UnityEvent OnYearChanged = new UnityEvent();
        
        // Time of day events
        /// <summary>Invoked when dawn starts.</summary>
        public static readonly UnityEvent OnDawnStarted = new UnityEvent();
        
        /// <summary>Invoked when morning starts.</summary>
        public static readonly UnityEvent OnMorningStarted = new UnityEvent();
        
        /// <summary>Invoked when afternoon starts.</summary>
        public static readonly UnityEvent OnAfternoonStarted = new UnityEvent();
        
        /// <summary>Invoked when evening starts.</summary>
        public static readonly UnityEvent OnEveningStarted = new UnityEvent();
        
        /// <summary>Invoked when night time starts.</summary>
        public static readonly UnityEvent OnNightTimeStarted = new UnityEvent();
        
        // Special time events
        /// <summary>Invoked when a specific hour is reached. Passes the hour (0-23) as parameter.</summary>
        public static readonly UnityEvent<int> OnSpecificHourReached = new UnityEvent<int>();
        
        /// <summary>
        /// Clears all event listeners. Useful for cleanup.
        /// </summary>
        public static void ClearAllEvents()
        {
            OnMinutePassed.RemoveAllListeners();
            OnHourPassed.RemoveAllListeners();
            OnDayStarted.RemoveAllListeners();
            OnDayEnded.RemoveAllListeners();
            OnNightStarted.RemoveAllListeners();
            OnNightEnded.RemoveAllListeners();
            OnSeasonChanged.RemoveAllListeners();
            OnYearChanged.RemoveAllListeners();
            OnDawnStarted.RemoveAllListeners();
            OnMorningStarted.RemoveAllListeners();
            OnAfternoonStarted.RemoveAllListeners();
            OnEveningStarted.RemoveAllListeners();
            OnNightTimeStarted.RemoveAllListeners();
            OnSpecificHourReached.RemoveAllListeners();
            OnSpringStarted.RemoveAllListeners();
            OnSummerStarted.RemoveAllListeners();
            OnFallStarted.RemoveAllListeners();
            OnWinterStarted.RemoveAllListeners();
        }
    }
}
