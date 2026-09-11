using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StardewTimeSystem.Samples
{
    /// <summary>
    /// Simple UI display for the time system.
    /// Attach this to a GameObject with a Text or TextMeshPro component.
    /// </summary>
    public class TimeDisplay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        [Tooltip("The Text component to display time on.")]
        private Text _timeText;
        
        [SerializeField]
        [Tooltip("The TextMeshPro component to display time on (alternative to Text).")]
        private TMP_Text _tmpTimeText;
        
        [SerializeField]
        [Tooltip("The Text component to display date on.")]
        private Text _dateText;
        
        [SerializeField]
        [Tooltip("The TextMeshPro component to display date on (alternative to Text).")]
        private TMP_Text _tmpDateText;
        
        [SerializeField]
        [Tooltip("The Text component to display season on.")]
        private Text _seasonText;
        
        [SerializeField]
        [Tooltip("The TextMeshPro component to display season on (alternative to Text).")]
        private TMP_Text _tmpSeasonText;
        
        [SerializeField]
        [Tooltip("The Text component to display time of day on.")]
        private Text _timeOfDayText;
        
        [SerializeField]
        [Tooltip("The TextMeshPro component to display time of day on (alternative to Text).")]
        private TMP_Text _tmpTimeOfDayText;
        
        [Header("Display Settings")]
        [SerializeField]
        [Tooltip("Whether to show the time.")]
        private bool _showTime = true;
        
        [SerializeField]
        [Tooltip("Whether to show the date.")]
        private bool _showDate = true;
        
        [SerializeField]
        [Tooltip("Whether to show the season.")]
        private bool _showSeason = true;
        
        [SerializeField]
        [Tooltip("Whether to show the time of day.")]
        private bool _showTimeOfDay = true;
        
        [Header("Format")]
        [SerializeField]
        [Tooltip("Custom format for time display. Use {0} for time.")]
        private string _timeFormat = "Time: {0}";
        
        [SerializeField]
        [Tooltip("Custom format for date display. Use {0} for date.")]
        private string _dateFormat = "Date: {0}";
        
        [SerializeField]
        [Tooltip("Custom format for season display. Use {0} for season.")]
        private string _seasonFormat = "Season: {0}";
        
        [SerializeField]
        [Tooltip("Custom format for time of day display. Use {0} for time of day.")]
        private string _timeOfDayFormat = "Time of Day: {0}";
        
        private void OnEnable()
        {
            // Subscribe to time events
            TimeEvents.OnMinutePassed.AddListener(UpdateDisplay);
            TimeEvents.OnHourPassed.AddListener(UpdateDisplay);
            TimeEvents.OnDayStarted.AddListener(UpdateDisplay);
            TimeEvents.OnSeasonChanged.AddListener(UpdateDisplay);
            TimeEvents.OnYearChanged.AddListener(UpdateDisplay);
            
            // Initial update
            UpdateDisplay();
        }
        
        private void OnDisable()
        {
            // Unsubscribe from time events
            TimeEvents.OnMinutePassed.RemoveListener(UpdateDisplay);
            TimeEvents.OnHourPassed.RemoveListener(UpdateDisplay);
            TimeEvents.OnDayStarted.RemoveListener(UpdateDisplay);
            TimeEvents.OnSeasonChanged.RemoveListener(UpdateDisplay);
            TimeEvents.OnYearChanged.RemoveListener(UpdateDisplay);
        }
        
        /// <summary>
        /// Updates all text displays with current time information.
        /// </summary>
        public void UpdateDisplay()
        {
            if (TimeSystem.Instance == null) return;
            
            if (_showTime)
            {
                string timeText = string.Format(_timeFormat, TimeSystem.Instance.FormattedTime);
                
                if (_timeText != null)
                {
                    _timeText.text = timeText;
                }
                
                if (_tmpTimeText != null)
                {
                    _tmpTimeText.text = timeText;
                }
            }
            
            if (_showDate)
            {
                string dateText = string.Format(_dateFormat, TimeSystem.Instance.FormattedDate);
                
                if (_dateText != null)
                {
                    _dateText.text = dateText;
                }
                
                if (_tmpDateText != null)
                {
                    _tmpDateText.text = dateText;
                }
            }
            
            if (_showSeason)
            {
                string seasonText = string.Format(_seasonFormat, TimeSystem.Instance.CurrentSeason.ToDisplayString());
                
                if (_seasonText != null)
                {
                    _seasonText.text = seasonText;
                }
                
                if (_tmpSeasonText != null)
                {
                    _tmpSeasonText.text = seasonText;
                }
            }
            
            if (_showTimeOfDay)
            {
                string timeOfDayText = string.Format(_timeOfDayFormat, TimeSystem.Instance.CurrentTimeOfDay.ToDisplayString());
                
                if (_timeOfDayText != null)
                {
                    _timeOfDayText.text = timeOfDayText;
                }
                
                if (_tmpTimeOfDayText != null)
                {
                    _tmpTimeOfDayText.text = timeOfDayText;
                }
            }
        }
    }
}
