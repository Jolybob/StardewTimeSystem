using UnityEngine;
using System;
using System.Collections.Generic;

namespace StardewTimeSystem
{
    /// <summary>
    /// Core time system managing the day/night cycle, seasons, and years.
    /// This is a singleton MonoBehaviour that should persist across scenes.
    /// 
    /// Features:
    /// - Configurable day duration (real-world seconds)
    /// - 24-hour clock with minutes
    /// - Day/Night cycle detection
    /// - Seasons (Spring, Summer, Fall, Winter)
    /// - Years
    /// - Time of Day (Dawn, Morning, Afternoon, Evening, Night)
    /// - Event-based notifications
    /// - Listener interface for direct callbacks
    /// - Pause/resume functionality
    /// - Time scaling (fast-forward for debugging)
    /// 
    /// Usage:
    /// 1. Add TimeSystem component to a GameObject in your first scene
    /// 2. Create a TimeData ScriptableObject via Create > Stardew > Time System > Time Data
    /// 3. Assign the TimeData to the TimeSystem component
    /// 4. Subscribe to TimeEvents or implement ITimeListener
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Stardew/Time System/Time System")]
    public class TimeSystem : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the TimeSystem.
        /// </summary>
        public static TimeSystem Instance { get; private set; }
        
        [Header("Configuration")]
        [SerializeField]
        [Tooltip("The time configuration data. Create via Create > Stardew > Time System > Time Data")]
        private TimeData _timeData;
        
        [Header("Debug")]
        [SerializeField]
        [Tooltip("Enable debug mode to control time manually.")]
        private bool _debugMode = false;
        
        [SerializeField]
        [Tooltip("Debug time scale multiplier (only in debug mode).")]
        private float _debugTimeScale = 10f;
        
        // Internal state
        private float _currentTimeSeconds = 0f;
        private int _currentMinute = 0;
        private int _currentHour = 0;
        private int _currentDay = 0;
        private int _currentSeason = 0;
        private int _currentYear = 0;
        private bool _isDaytime = true;
        private TimeOfDay _currentTimeOfDay = TimeOfDay.Morning;
        private TimeOfDay _previousTimeOfDay = TimeOfDay.Morning;
        
        // Time listener registry
        private readonly List<ITimeListener> _timeListeners = new List<ITimeListener>();
        
        // Cached values
        private float MinuteDuration => _timeData.MinuteDurationSeconds * TimeScale;
        private float HourDuration => _timeData.HourDurationSeconds * TimeScale;
        private float TimeScale => _debugMode ? _debugTimeScale : _timeData.TimeScale;
        
        // Properties
        /// <summary>Current minute (0-59)</summary>
        public int CurrentMinute => _currentMinute;
        
        /// <summary>Current hour (0-23)</summary>
        public int CurrentHour => _currentHour;
        
        /// <summary>Current day of the season (1-28)</summary>
        public int CurrentDay => _currentDay + 1;
        
        /// <summary>Current season (0-3)</summary>
        public Season CurrentSeason => (Season)_currentSeason;
        
        /// <summary>Current year</summary>
        public int CurrentYear => _currentYear + 1;
        
        /// <summary>Whether it's currently daytime</summary>
        public bool IsDaytime => _isDaytime;
        
        /// <summary>Current time of day</summary>
        public TimeOfDay CurrentTimeOfDay => _currentTimeOfDay;
        
        /// <summary>Total days passed since the start</summary>
        public int TotalDays => _currentYear * _timeData.TotalDaysInYear + 
                                _currentSeason * _timeData.DaysInSeason + 
                                _currentDay;
        
        /// <summary>Formatted time string (e.g., "6:30 AM")</summary>
        public string FormattedTime => FormatTime(_currentHour, _currentMinute);
        
        /// <summary>Formatted date string (e.g., "Spring 1, Year 1")</summary>
        public string FormattedDate => $"{CurrentSeason.ToDisplayString()} {CurrentDay}, Year {CurrentYear}";
        
        /// <summary>Formatted full date/time string</summary>
        public string FormattedDateTime => $"{FormattedDate} - {FormattedTime}";
        
        /// <summary>Time data configuration</summary>
        public TimeData TimeData => _timeData;
        
        /// <summary>Whether the time system is currently paused.</summary>
        public bool IsPaused { get; private set; } = false;
        
        /// <summary>Event triggered when the time system is initialized.</summary>
        public event Action OnInitialized;
        
        /// <summary>Event triggered when the time system is destroyed.</summary>
        public event Action OnDestroyed;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple TimeSystem instances detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load default TimeData if not set
            if (_timeData == null)
            {
                _timeData = Resources.Load<TimeData>("TimeData");
                if (_timeData == null)
                {
                    _timeData = ScriptableObject.CreateInstance<TimeData>();
                    Debug.LogWarning("No TimeData found. Created a default instance. " +
                        "Create one via Create > Stardew > Time System > Time Data");
                }
            }
            
            // Initialize time from TimeData start values
            InitializeTime();
            
            // Trigger initialization event
            OnInitialized?.Invoke();
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] Initialized. Start: {FormattedDateTime}");
            }
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
            
            OnDestroyed?.Invoke();
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log("[TimeSystem] Destroyed.");
            }
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (_timeData.PauseWhenNotFocused)
            {
                IsPaused = pauseStatus;
                
                if (_timeData.EnableDebugLogs)
                {
                    Debug.Log($"[TimeSystem] Application {(pauseStatus ? "paused" : "resumed")}. Time {(IsPaused ? "paused" : "resumed")}.");
                }
            }
        }
        
        private void Update()
        {
            if (IsPaused) return;
            
            // Accumulate time
            _currentTimeSeconds += Time.deltaTime;
            
            // Check if we've passed a minute
            while (_currentTimeSeconds >= MinuteDuration)
            {
                _currentTimeSeconds -= MinuteDuration;
                AdvanceMinute();
            }
        }
        
        /// <summary>
        /// Initializes the time system with the start values from TimeData.
        /// </summary>
        private void InitializeTime()
        {
            _currentYear = _timeData.StartYear - 1; // Convert to 0-based
            _currentSeason = _timeData.StartSeason;
            _currentDay = _timeData.StartDay - 1; // Convert to 0-based
            _currentHour = _timeData.StartHour;
            _currentMinute = _timeData.StartMinute;
            
            // Calculate initial state
            _isDaytime = _timeData.IsDaytime(_currentHour);
            _currentTimeOfDay = _timeData.GetTimeOfDay(_currentHour);
            _previousTimeOfDay = _currentTimeOfDay;
            
            // Trigger initial events
            UpdateDayNightState();
            UpdateTimeOfDayState();
        }
        
        /// <summary>
        /// Advances the time by one minute and handles all cascading updates.
        /// </summary>
        private void AdvanceMinute()
        {
            _currentMinute++;
            
            // Check if we've passed an hour
            if (_currentMinute >= _timeData.MinutesInHour)
            {
                _currentMinute = 0;
                _currentHour++;
                
                // Trigger hour events
                TriggerHourEvents();
                
                // Check if we've passed a day
                if (_currentHour >= _timeData.HoursInDay)
                {
                    _currentHour = 0;
                    _currentDay++;
                    
                    // Check if we've passed a season
                    if (_currentDay >= _timeData.DaysInSeason)
                    {
                        _currentDay = 0;
                        _currentSeason++;
                        
                        // Check if we've passed a year
                        if (_currentSeason >= _timeData.SeasonsInYear)
                        {
                            _currentSeason = 0;
                            _currentYear++;
                            
                            TriggerYearEvents();
                        }
                        
                        TriggerSeasonEvents();
                    }
                    
                    TriggerDayEvents();
                }
            }
            
            // Update state
            bool wasDaytime = _isDaytime;
            TimeOfDay previousTimeOfDay = _currentTimeOfDay;
            
            _isDaytime = _timeData.IsDaytime(_currentHour);
            _currentTimeOfDay = _timeData.GetTimeOfDay(_currentHour);
            
            // Trigger minute events
            TimeEvents.OnMinutePassed.Invoke();
            foreach (var listener in _timeListeners)
            {
                listener.OnMinutePassed(_currentHour, _currentMinute);
            }
            
            // Check for state changes
            if (wasDaytime != _isDaytime)
            {
                UpdateDayNightState();
            }
            
            if (previousTimeOfDay != _currentTimeOfDay)
            {
                UpdateTimeOfDayState();
            }
            
            _previousTimeOfDay = _currentTimeOfDay;
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] Minute: {_currentHour:D2}:{_currentMinute:D2} | Day: {CurrentDay} | Season: {CurrentSeason} | Year: {CurrentYear}");
            }
        }
        
        /// <summary>
        /// Triggers all hour-related events.
        /// </summary>
        private void TriggerHourEvents()
        {
            TimeEvents.OnHourPassed.Invoke();
            TimeEvents.OnSpecificHourReached.Invoke(_currentHour);
            
            foreach (var listener in _timeListeners)
            {
                listener.OnHourPassed(_currentHour);
            }
        }
        
        /// <summary>
        /// Triggers all day-related events.
        /// </summary>
        private void TriggerDayEvents()
        {
            TimeEvents.OnDayStarted.Invoke();
            TimeEvents.OnNightEnded.Invoke();
            
            foreach (var listener in _timeListeners)
            {
                listener.OnDayStarted(CurrentDay, CurrentSeason, CurrentYear);
            }
            
            // Day just started, so it's daytime
            _isDaytime = true;
            UpdateDayNightState();
        }
        
        /// <summary>
        /// Triggers all season-related events.
        /// </summary>
        private void TriggerSeasonEvents()
        {
            TimeEvents.OnSeasonChanged.Invoke();
            
            foreach (var listener in _timeListeners)
            {
                listener.OnSeasonChanged(CurrentSeason, CurrentYear);
            }
            
            // Trigger season-specific events
            switch (CurrentSeason)
            {
                case Season.Spring:
                    TimeEvents.OnSpringStarted.Invoke();
                    break;
                case Season.Summer:
                    TimeEvents.OnSummerStarted.Invoke();
                    break;
                case Season.Fall:
                    TimeEvents.OnFallStarted.Invoke();
                    break;
                case Season.Winter:
                    TimeEvents.OnWinterStarted.Invoke();
                    break;
            }
        }
        
        /// <summary>
        /// Triggers all year-related events.
        /// </summary>
        private void TriggerYearEvents()
        {
            TimeEvents.OnYearChanged.Invoke();
            
            foreach (var listener in _timeListeners)
            {
                listener.OnYearChanged(CurrentYear);
            }
        }
        
        /// <summary>
        /// Updates day/night state and triggers appropriate events.
        /// </summary>
        private void UpdateDayNightState()
        {
            if (_isDaytime)
            {
                TimeEvents.OnNightEnded.Invoke();
            }
            else
            {
                TimeEvents.OnNightStarted.Invoke();
            }
        }
        
        /// <summary>
        /// Updates time of day state and triggers appropriate events.
        /// </summary>
        private void UpdateTimeOfDayState()
        {
            switch (_currentTimeOfDay)
            {
                case TimeOfDay.Dawn:
                    TimeEvents.OnDawnStarted.Invoke();
                    break;
                case TimeOfDay.Morning:
                    TimeEvents.OnMorningStarted.Invoke();
                    break;
                case TimeOfDay.Afternoon:
                    TimeEvents.OnAfternoonStarted.Invoke();
                    break;
                case TimeOfDay.Evening:
                    TimeEvents.OnEveningStarted.Invoke();
                    break;
                case TimeOfDay.Night:
                    TimeEvents.OnNightTimeStarted.Invoke();
                    break;
            }
        }
        
        /// <summary>
        /// Registers a time listener to receive callbacks.
        /// </summary>
        /// <param name="listener">The listener to register</param>
        public void RegisterListener(ITimeListener listener)
        {
            if (listener == null) return;
            if (!_timeListeners.Contains(listener))
            {
                _timeListeners.Add(listener);
            }
        }
        
        /// <summary>
        /// Unregisters a time listener.
        /// </summary>
        /// <param name="listener">The listener to unregister</param>
        public void UnregisterListener(ITimeListener listener)
        {
            if (listener == null) return;
            _timeListeners.Remove(listener);
        }
        
        /// <summary>
        /// Pauses the time system.
        /// </summary>
        public void Pause()
        {
            IsPaused = true;
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log("[TimeSystem] Paused.");
            }
        }
        
        /// <summary>
        /// Resumes the time system.
        /// </summary>
        public void Resume()
        {
            IsPaused = false;
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log("[TimeSystem] Resumed.");
            }
        }
        
        /// <summary>
        /// Toggles pause state.
        /// </summary>
        public void TogglePause()
        {
            IsPaused = !IsPaused;
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] {(IsPaused ? "Paused" : "Resumed")}.");
            }
        }
        
        /// <summary>
        /// Sets the time to a specific hour and minute.
        /// </summary>
        /// <param name="hour">Hour (0-23)</param>
        /// <param name="minute">Minute (0-59)</param>
        public void SetTime(int hour, int minute = 0)
        {
            _currentHour = Mathf.Clamp(hour, 0, _timeData.HoursInDay - 1);
            _currentMinute = Mathf.Clamp(minute, 0, _timeData.MinutesInHour - 1);
            _currentTimeSeconds = 0f;
            
            // Update state
            _isDaytime = _timeData.IsDaytime(_currentHour);
            _currentTimeOfDay = _timeData.GetTimeOfDay(_currentHour);
            _previousTimeOfDay = _currentTimeOfDay;
            
            UpdateDayNightState();
            UpdateTimeOfDayState();
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] Time set to {hour:D2}:{minute:D2}.");
            }
        }
        
        /// <summary>
        /// Sets the date to a specific day, season, and year.
        /// </summary>
        /// <param name="day">Day of season (1-28)</param>
        /// <param name="season">Season</param>
        /// <param name="year">Year</param>
        public void SetDate(int day, Season season, int year)
        {
            _currentDay = Mathf.Clamp(day, 1, _timeData.DaysInSeason) - 1;
            _currentSeason = (int)season;
            _currentYear = year - 1; // Convert to 0-based
            
            _currentTimeSeconds = 0f;
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] Date set to {season} {day}, Year {year}.");
            }
        }
        
        /// <summary>
        /// Advances the time by the specified number of minutes.
        /// </summary>
        /// <param name="minutes">Number of minutes to advance</param>
        public void AdvanceMinutes(int minutes)
        {
            for (int i = 0; i < minutes; i++)
            {
                AdvanceMinute();
            }
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] Advanced by {minutes} minutes. Now: {FormattedDateTime}");
            }
        }
        
        /// <summary>
        /// Advances the time by the specified number of hours.
        /// </summary>
        /// <param name="hours">Number of hours to advance</param>
        public void AdvanceHours(int hours)
        {
            AdvanceMinutes(hours * _timeData.MinutesInHour);
        }
        
        /// <summary>
        /// Advances the time by the specified number of days.
        /// </summary>
        /// <param name="days">Number of days to advance</param>
        public void AdvanceDays(int days)
        {
            for (int i = 0; i < days; i++)
            {
                // Advance to next day
                _currentDay++;
                if (_currentDay >= _timeData.DaysInSeason)
                {
                    _currentDay = 0;
                    _currentSeason++;
                    if (_currentSeason >= _timeData.SeasonsInYear)
                    {
                        _currentSeason = 0;
                        _currentYear++;
                        TriggerYearEvents();
                    }
                    TriggerSeasonEvents();
                }
                
                TriggerDayEvents();
            }
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log($"[TimeSystem] Advanced by {days} days. Now: {FormattedDate}");
            }
        }
        
        /// <summary>
        /// Formats the time as a 12-hour clock string (e.g., "6:30 AM" or "2:45 PM").
        /// </summary>
        /// <param name="hour">Hour (0-23)</param>
        /// <param name="minute">Minute (0-59)</param>
        /// <returns>Formatted time string</returns>
        private static string FormatTime(int hour, int minute)
        {
            int hour12 = hour % 12;
            if (hour12 == 0) hour12 = 12;
            string period = hour < 12 ? "AM" : "PM";
            return $"{hour12}:{minute:D2} {period}";
        }
        
        /// <summary>
        /// Gets the progress through the current day as a value between 0 and 1.
        /// 0 = start of day, 1 = end of day.
        /// </summary>
        /// <returns>Day progress (0-1)</returns>
        public float GetDayProgress()
        {
            float totalMinutes = _currentHour * _timeData.MinutesInHour + _currentMinute;
            float maxMinutes = _timeData.HoursInDay * _timeData.MinutesInHour;
            return totalMinutes / maxMinutes;
        }
        
        /// <summary>
        /// Gets the progress through the current season as a value between 0 and 1.
        /// </summary>
        /// <returns>Season progress (0-1)</returns>
        public float GetSeasonProgress()
        {
            return (float)_currentDay / _timeData.DaysInSeason;
        }
        
        /// <summary>
        /// Gets the progress through the current year as a value between 0 and 1.
        /// </summary>
        /// <returns>Year progress (0-1)</returns>
        public float GetYearProgress()
        {
            float daysPassed = _currentSeason * _timeData.DaysInSeason + _currentDay;
            return daysPassed / _timeData.TotalDaysInYear;
        }
        
        /// <summary>
        /// Gets the time remaining until the next hour in seconds.
        /// </summary>
        /// <returns>Seconds until next hour</returns>
        public float GetSecondsUntilNextHour()
        {
            float minutesPassed = _currentTimeSeconds / MinuteDuration;
            float minutesUntilNextHour = _timeData.MinutesInHour - minutesPassed;
            return minutesUntilNextHour * MinuteDuration;
        }
        
        /// <summary>
        /// Gets the time remaining until the next day in seconds.
        /// </summary>
        /// <returns>Seconds until next day</returns>
        public float GetSecondsUntilNextDay()
        {
            float minutesPassed = _currentHour * _timeData.MinutesInHour + _currentMinute + 
                                  (_currentTimeSeconds / MinuteDuration);
            float minutesUntilNextDay = (_timeData.HoursInDay * _timeData.MinutesInHour) - minutesPassed;
            return minutesUntilNextDay * MinuteDuration;
        }
        
        /// <summary>
        /// Resets the time system to the initial state from TimeData.
        /// </summary>
        public void Reset()
        {
            _currentTimeSeconds = 0f;
            InitializeTime();
            
            if (_timeData.EnableDebugLogs)
            {
                Debug.Log("[TimeSystem] Reset to initial state.");
            }
        }
    }
}
