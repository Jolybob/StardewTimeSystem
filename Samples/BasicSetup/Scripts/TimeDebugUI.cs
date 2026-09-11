using UnityEngine;
using UnityEngine.UI;

namespace StardewTimeSystem.Samples
{
    /// <summary>
    /// Debug UI for controlling and monitoring the time system.
    /// This script provides buttons to control time and displays debug information.
    /// </summary>
    public class TimeDebugUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        [Tooltip("Button to pause time.")]
        private Button _pauseButton;
        
        [SerializeField]
        [Tooltip("Button to resume time.")]
        private Button _resumeButton;
        
        [SerializeField]
        [Tooltip("Button to advance one hour.")]
        private Button _advanceHourButton;
        
        [SerializeField]
        [Tooltip("Button to advance one day.")]
        private Button _advanceDayButton;
        
        [SerializeField]
        [Tooltip("Button to advance one season.")]
        private Button _advanceSeasonButton;
        
        [SerializeField]
        [Tooltip("Button to reset time.")]
        private Button _resetButton;
        
        [SerializeField]
        [Tooltip("Dropdown for setting time of day.")]
        private Dropdown _timeOfDayDropdown;
        
        [SerializeField]
        [Tooltip("Dropdown for setting season.")]
        private Dropdown _seasonDropdown;
        
        [SerializeField]
        [Tooltip("Text to display debug info.")]
        private Text _debugInfoText;
        
        private void OnEnable()
        {
            // Set up button callbacks
            if (_pauseButton != null)
            {
                _pauseButton.onClick.AddListener(OnPauseClicked);
            }
            
            if (_resumeButton != null)
            {
                _resumeButton.onClick.AddListener(OnResumeClicked);
            }
            
            if (_advanceHourButton != null)
            {
                _advanceHourButton.onClick.AddListener(OnAdvanceHourClicked);
            }
            
            if (_advanceDayButton != null)
            {
                _advanceDayButton.onClick.AddListener(OnAdvanceDayClicked);
            }
            
            if (_advanceSeasonButton != null)
            {
                _advanceSeasonButton.onClick.AddListener(OnAdvanceSeasonClicked);
            }
            
            if (_resetButton != null)
            {
                _resetButton.onClick.AddListener(OnResetClicked);
            }
            
            if (_timeOfDayDropdown != null)
            {
                _timeOfDayDropdown.onValueChanged.AddListener(OnTimeOfDayChanged);
                
                // Populate dropdown
                _timeOfDayDropdown.ClearOptions();
                _timeOfDayDropdown.AddOptions(new System.Collections.Generic.List<string>
                {
                    "Dawn",
                    "Morning",
                    "Afternoon",
                    "Evening",
                    "Night"
                });
            }
            
            if (_seasonDropdown != null)
            {
                _seasonDropdown.onValueChanged.AddListener(OnSeasonChanged);
                
                // Populate dropdown
                _seasonDropdown.ClearOptions();
                _seasonDropdown.AddOptions(new System.Collections.Generic.List<string>
                {
                    "Spring",
                    "Summer",
                    "Fall",
                    "Winter"
                });
            }
            
            // Subscribe to time events for debug info
            TimeEvents.OnMinutePassed.AddListener(UpdateDebugInfo);
            TimeEvents.OnHourPassed.AddListener(UpdateDebugInfo);
            TimeEvents.OnDayStarted.AddListener(UpdateDebugInfo);
            TimeEvents.OnSeasonChanged.AddListener(UpdateDebugInfo);
            TimeEvents.OnYearChanged.AddListener(UpdateDebugInfo);
            
            // Initial update
            UpdateDebugInfo();
        }
        
        private void OnDisable()
        {
            // Clean up button callbacks
            if (_pauseButton != null)
            {
                _pauseButton.onClick.RemoveListener(OnPauseClicked);
            }
            
            if (_resumeButton != null)
            {
                _resumeButton.onClick.RemoveListener(OnResumeClicked);
            }
            
            if (_advanceHourButton != null)
            {
                _advanceHourButton.onClick.RemoveListener(OnAdvanceHourClicked);
            }
            
            if (_advanceDayButton != null)
            {
                _advanceDayButton.onClick.RemoveListener(OnAdvanceDayClicked);
            }
            
            if (_advanceSeasonButton != null)
            {
                _advanceSeasonButton.onClick.RemoveListener(OnAdvanceSeasonClicked);
            }
            
            if (_resetButton != null)
            {
                _resetButton.onClick.RemoveListener(OnResetClicked);
            }
            
            if (_timeOfDayDropdown != null)
            {
                _timeOfDayDropdown.onValueChanged.RemoveListener(OnTimeOfDayChanged);
            }
            
            if (_seasonDropdown != null)
            {
                _seasonDropdown.onValueChanged.RemoveListener(OnSeasonChanged);
            }
            
            // Unsubscribe from time events
            TimeEvents.OnMinutePassed.RemoveListener(UpdateDebugInfo);
            TimeEvents.OnHourPassed.RemoveListener(UpdateDebugInfo);
            TimeEvents.OnDayStarted.RemoveListener(UpdateDebugInfo);
            TimeEvents.OnSeasonChanged.RemoveListener(UpdateDebugInfo);
            TimeEvents.OnYearChanged.RemoveListener(UpdateDebugInfo);
        }
        
        private void UpdateDebugInfo()
        {
            if (TimeSystem.Instance == null || _debugInfoText == null) return;
            
            string info = $"Time System Debug Info\n\n";
            info += $"Status: {(TimeSystem.Instance.IsPaused ? "PAUSED" : "Running")}\n";
            info += $"Time: {TimeSystem.Instance.FormattedTime}\n";
            info += $"Date: {TimeSystem.Instance.FormattedDate}\n";
            info += $"Time of Day: {TimeSystem.Instance.CurrentTimeOfDay}\n";
            info += $"Is Daytime: {TimeSystem.Instance.IsDaytime}\n";
            info += $"Day Progress: {TimeSystem.Instance.GetDayProgress():P2}\n";
            info += $"Season Progress: {TimeSystem.Instance.GetSeasonProgress():P2}\n";
            info += $"Year Progress: {TimeSystem.Instance.GetYearProgress():P2}\n";
            info += $"Total Days: {TimeSystem.Instance.TotalDays}\n";
            info += $"Next Hour: {TimeSystem.Instance.GetSecondsUntilNextHour():F1}s\n";
            info += $"Next Day: {TimeSystem.Instance.GetSecondsUntilNextDay():F1}s";
            
            _debugInfoText.text = info;
        }
        
        // Button click handlers
        private void OnPauseClicked()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.Pause();
                UpdateDebugInfo();
            }
        }
        
        private void OnResumeClicked()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.Resume();
                UpdateDebugInfo();
            }
        }
        
        private void OnAdvanceHourClicked()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.AdvanceHours(1);
            }
        }
        
        private void OnAdvanceDayClicked()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.AdvanceDays(1);
            }
        }
        
        private void OnAdvanceSeasonClicked()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.AdvanceDays(TimeSystem.Instance.TimeData.DaysInSeason);
            }
        }
        
        private void OnResetClicked()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.Reset();
            }
        }
        
        private void OnTimeOfDayChanged(int index)
        {
            if (TimeSystem.Instance != null)
            {
                TimeOfDay timeOfDay = (TimeOfDay)index;
                TimeData timeData = TimeSystem.Instance.TimeData;
                
                // Find the start hour for this time of day
                int hour = timeOfDay switch
                {
                    TimeOfDay.Dawn => timeData.DawnStartHour,
                    TimeOfDay.Morning => timeData.MorningStartHour,
                    TimeOfDay.Afternoon => timeData.AfternoonStartHour,
                    TimeOfDay.Evening => timeData.EveningStartHour,
                    TimeOfDay.Night => timeData.EveningEndHour,
                    _ => 0
                };
                
                TimeSystem.Instance.SetTime(hour, 0);
            }
        }
        
        private void OnSeasonChanged(int index)
        {
            if (TimeSystem.Instance != null)
            {
                Season season = (Season)index;
                TimeSystem.Instance.SetDate(1, season, TimeSystem.Instance.CurrentYear);
            }
        }
    }
}
