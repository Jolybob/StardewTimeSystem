using System;

namespace StardewTimeSystem
{
    /// <summary>
    /// Interface for objects that want to receive time update callbacks.
    /// Implement this to react to time changes without using events.
    /// </summary>
    public interface ITimeListener : IDisposable
    {
        /// <summary>
        /// Called every minute.
        /// </summary>
        /// <param name="currentHour">Current hour (0-23)</param>
        /// <param name="currentMinute">Current minute (0-59)</param>
        void OnMinutePassed(int currentHour, int currentMinute);
        
        /// <summary>
        /// Called every hour.
        /// </summary>
        /// <param name="currentHour">Current hour (0-23)</param>
        void OnHourPassed(int currentHour);
        
        /// <summary>
        /// Called when a new day starts.
        /// </summary>
        /// <param name="day">Current day of season (1-28)</param>
        /// <param name="season">Current season</param>
        /// <param name="year">Current year</param>
        void OnDayStarted(int day, Season season, int year);
        
        /// <summary>
        /// Called when a day ends.
        /// </summary>
        /// <param name="day">Current day of season (1-28)</param>
        /// <param name="season">Current season</param>
        /// <param name="year">Current year</param>
        void OnDayEnded(int day, Season season, int year);
        
        /// <summary>
        /// Called when the season changes.
        /// </summary>
        /// <param name="newSeason">New season</param>
        /// <param name="year">Current year</param>
        void OnSeasonChanged(Season newSeason, int year);
        
        /// <summary>
        /// Called when the year changes.
        /// </summary>
        /// <param name="newYear">New year</param>
        void OnYearChanged(int newYear);
    }

    /// <summary>
    /// Base implementation of ITimeListener for easier implementation.
    /// Override only the methods you need.
    /// </summary>
    public abstract class TimeListenerBase : ITimeListener
    {
        /// <summary>
        /// Registers this listener with the TimeSystem.
        /// </summary>
        protected TimeListenerBase()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.RegisterListener(this);
            }
        }

        /// <summary>
        /// Called every minute.
        /// </summary>
        public virtual void OnMinutePassed(int currentHour, int currentMinute) { }
        
        /// <summary>
        /// Called every hour.
        /// </summary>
        public virtual void OnHourPassed(int currentHour) { }
        
        /// <summary>
        /// Called when a new day starts.
        /// </summary>
        public virtual void OnDayStarted(int day, Season season, int year) { }
        
        /// <summary>
        /// Called when a day ends.
        /// </summary>
        public virtual void OnDayEnded(int day, Season season, int year) { }
        
        /// <summary>
        /// Called when the season changes.
        /// </summary>
        public virtual void OnSeasonChanged(Season newSeason, int year) { }
        
        /// <summary>
        /// Called when the year changes.
        /// </summary>
        public virtual void OnYearChanged(int newYear) { }

        /// <summary>
        /// Disposes the listener and unregisters it from TimeSystem.
        /// </summary>
        public virtual void Dispose()
        {
            if (TimeSystem.Instance != null)
            {
                TimeSystem.Instance.UnregisterListener(this);
            }
        }
    }
}
