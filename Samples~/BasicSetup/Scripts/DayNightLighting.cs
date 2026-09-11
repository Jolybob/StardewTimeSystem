using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StardewTimeSystem.Samples
{
    /// <summary>
    /// Adjusts lighting based on the time of day.
    /// This script demonstrates how to use the time system to control 2D lighting.
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class DayNightLighting : MonoBehaviour
    {
        [Header("Light Settings")]
        [SerializeField]
        [Tooltip("Intensity of the light during day.")]
        [Range(0f, 10f)]
        private float _dayIntensity = 1f;
        
        [SerializeField]
        [Tooltip("Intensity of the light during night.")]
        [Range(0f, 10f)]
        private float _nightIntensity = 0.2f;
        
        [SerializeField]
        [Tooltip("Color of the light during day.")]
        private Color _dayColor = new Color(1f, 0.95f, 0.9f, 1f);
        
        [SerializeField]
        [Tooltip("Color of the light during night.")]
        private Color _nightColor = new Color(0.3f, 0.4f, 0.8f, 1f);
        
        [SerializeField]
        [Tooltip("Color of the light during dawn.")]
        private Color _dawnColor = new Color(1f, 0.8f, 0.6f, 1f);
        
        [SerializeField]
        [Tooltip("Color of the light during evening.")]
        private Color _eveningColor = new Color(0.8f, 0.6f, 0.4f, 1f);
        
        [Header("Transition Settings")]
        [SerializeField]
        [Tooltip("How quickly the light transitions between states.")]
        [Range(0.1f, 10f)]
        private float _transitionSpeed = 1f;
        
        private Light2D _light;
        private Color _targetColor;
        private float _targetIntensity;
        
        private void Awake()
        {
            _light = GetComponent<Light2D>();
            
            // Set initial values
            if (TimeSystem.Instance != null)
            {
                UpdateTargetValues();
            }
        }
        
        private void OnEnable()
        {
            // Subscribe to time of day events
            TimeEvents.OnDawnStarted.AddListener(OnDawnStarted);
            TimeEvents.OnMorningStarted.AddListener(OnMorningStarted);
            TimeEvents.OnAfternoonStarted.AddListener(OnAfternoonStarted);
            TimeEvents.OnEveningStarted.AddListener(OnEveningStarted);
            TimeEvents.OnNightTimeStarted.AddListener(OnNightTimeStarted);
            TimeEvents.OnDayStarted.AddListener(OnDayStarted);
            
            // Also subscribe to minute passed for smooth transitions
            TimeEvents.OnMinutePassed.AddListener(OnMinutePassed);
            
            // Initialize
            if (TimeSystem.Instance != null)
            {
                UpdateTargetValues();
            }
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            TimeEvents.OnDawnStarted.RemoveListener(OnDawnStarted);
            TimeEvents.OnMorningStarted.RemoveListener(OnMorningStarted);
            TimeEvents.OnAfternoonStarted.RemoveListener(OnAfternoonStarted);
            TimeEvents.OnEveningStarted.RemoveListener(OnEveningStarted);
            TimeEvents.OnNightTimeStarted.RemoveListener(OnNightTimeStarted);
            TimeEvents.OnDayStarted.RemoveListener(OnDayStarted);
            TimeEvents.OnMinutePassed.RemoveListener(OnMinutePassed);
        }
        
        private void Update()
        {
            if (_light == null) return;
            
            // Smoothly transition to target values
            _light.color = Color.Lerp(_light.color, _targetColor, Time.deltaTime * _transitionSpeed);
            _light.intensity = Mathf.Lerp(_light.intensity, _targetIntensity, Time.deltaTime * _transitionSpeed);
        }
        
        /// <summary>
        /// Updates the target color and intensity based on the current time of day.
        /// </summary>
        private void UpdateTargetValues()
        {
            if (TimeSystem.Instance == null) return;
            
            switch (TimeSystem.Instance.CurrentTimeOfDay)
            {
                case TimeOfDay.Dawn:
                    _targetColor = _dawnColor;
                    _targetIntensity = Mathf.Lerp(_nightIntensity, _dayIntensity, 0.5f);
                    break;
                case TimeOfDay.Morning:
                case TimeOfDay.Afternoon:
                    _targetColor = _dayColor;
                    _targetIntensity = _dayIntensity;
                    break;
                case TimeOfDay.Evening:
                    _targetColor = _eveningColor;
                    _targetIntensity = Mathf.Lerp(_dayIntensity, _nightIntensity, 0.5f);
                    break;
                case TimeOfDay.Night:
                    _targetColor = _nightColor;
                    _targetIntensity = _nightIntensity;
                    break;
            }
        }
        
        // Event handlers
        private void OnDawnStarted() => UpdateTargetValues();
        private void OnMorningStarted() => UpdateTargetValues();
        private void OnAfternoonStarted() => UpdateTargetValues();
        private void OnEveningStarted() => UpdateTargetValues();
        private void OnNightTimeStarted() => UpdateTargetValues();
        private void OnDayStarted() => UpdateTargetValues();
        private void OnMinutePassed() => UpdateTargetValues();
    }
}
