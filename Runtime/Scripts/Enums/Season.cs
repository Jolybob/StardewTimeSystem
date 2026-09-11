namespace StardewTimeSystem
{
    /// <summary>
    /// Represents the four seasons in a year.
    /// </summary>
    public enum Season
    {
        /// <summary>Spring (season 0)</summary>
        Spring = 0,
        /// <summary>Summer (season 1)</summary>
        Summer = 1,
        /// <summary>Fall/Autumn (season 2)</summary>
        Fall = 2,
        /// <summary>Winter (season 3)</summary>
        Winter = 3
    }

    /// <summary>
    /// Extension methods for Season enum.
    /// </summary>
    public static class SeasonExtensions
    {
        /// <summary>
        /// Gets the next season in the cycle.
        /// </summary>
        /// <param name="season">Current season</param>
        /// <returns>Next season</returns>
        public static Season Next(this Season season)
        {
            return (Season)(((int)season + 1) % 4);
        }

        /// <summary>
        /// Gets the previous season in the cycle.
        /// </summary>
        /// <param name="season">Current season</param>
        /// <returns>Previous season</returns>
        public static Season Previous(this Season season)
        {
            return (Season)(((int)season - 1 + 4) % 4);
        }

        /// <summary>
        /// Converts season to a display-friendly string.
        /// </summary>
        /// <param name="season">Season to convert</param>
        /// <returns>Display string</returns>
        public static string ToDisplayString(this Season season)
        {
            return season switch
            {
                Season.Spring => "Spring",
                Season.Summer => "Summer",
                Season.Fall => "Fall",
                Season.Winter => "Winter",
                _ => season.ToString()
            };
        }
    }
}
