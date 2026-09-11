# Stardew Time System

A **flexible day/night cycle and time management system** for Unity games, inspired by Stardew Valley.

This package provides a complete time system with:
- Configurable day duration (real-world seconds)
- 24-hour clock with minutes
- Day/Night cycle detection
- Seasons (Spring, Summer, Fall, Winter)
- Years
- Time of Day (Dawn, Morning, Afternoon, Evening, Night)
- Event-based notifications
- Listener interface for direct callbacks
- Pause/resume functionality
- Time scaling (fast-forward for debugging)

## 🚀 Installation

### Via UPM (Unity Package Manager)

1. **Open Package Manager** in Unity (Window > Package Manager)
2. Click the **+** button in the top-left corner
3. Select **Add package from git URL...**
4. Enter the package URL (when published)

### Manual Installation

1. Download the package from the repository
2. Import the `.unitypackage` file into your Unity project
3. Or copy the `com.stardew.timesystem` folder into your project's `Packages` directory

## 📦 Package Structure

```
com.stardew.timesystem/
├── Runtime/
│   ├── Scripts/
│   │   ├── Enums/
│   │   │   ├── Season.cs          # Season enum with extensions
│   │   │   └── TimeOfDay.cs       # TimeOfDay enum with extensions
│   │   ├── Interfaces/
│   │   │   └── ITimeListener.cs    # Listener interface for callbacks
│   │   ├── TimeSystem.cs          # Core time system MonoBehaviour
│   │   ├── TimeData.cs            # ScriptableObject for configuration
│   │   └── TimeEvents.cs          # Static event bus
│   └── StardewTimeSystem.asmdef   # Assembly definition
├── Samples/
│   └── BasicSetup/               # Example scene and scripts
├── Documentation/
│   └── README.md                  # This file
└── package.json                   # UPM package manifest
```

## 🔧 Setup

### Step 1: Add TimeSystem to Your Scene

1. Create an empty GameObject in your first scene (e.g., "GameManager")
2. Add the `TimeSystem` component to it
3. Ensure the GameObject has `DontDestroyOnLoad` enabled (it does by default)

### Step 2: Create TimeData Configuration

1. Right-click in the Project window
2. Select **Create > Stardew > Time System > Time Data**
3. This creates a `TimeData` ScriptableObject
4. Configure the settings in the Inspector:
   - **Day Duration Seconds**: How long a full day lasts in real-world seconds (default: 120 = 2 minutes)
   - **Hours In Day**: Number of hours in a day (default: 24)
   - **Minutes In Hour**: Number of minutes in an hour (default: 60)
   - **Day/Night Cycle**: When day and night start (default: 6 AM and 6 PM)
   - **Time of Day Ranges**: Customize dawn, morning, afternoon, evening, night ranges
   - **Seasons**: Days per season and seasons per year
   - **Start Time**: Initial time when the game begins
   - **Time Scale**: Multiplier for time speed (1.0 = normal, 2.0 = 2x faster)
   - **Pause When Not Focused**: Whether time pauses when the game window is not focused

5. Assign the `TimeData` asset to the `TimeSystem` component's `_timeData` field

### Step 3: Subscribe to Events (Optional)

There are two ways to receive time updates:

#### Option A: Using TimeEvents (Recommended)

```csharp
using StardewTimeSystem;
using UnityEngine;

public class MyTimeListener : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to events
        TimeEvents.OnHourPassed.AddListener(OnHourPassed);
        TimeEvents.OnDayStarted.AddListener(OnDayStarted);
        TimeEvents.OnSeasonChanged.AddListener(OnSeasonChanged);
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events
        TimeEvents.OnHourPassed.RemoveListener(OnHourPassed);
        TimeEvents.OnDayStarted.RemoveListener(OnDayStarted);
        TimeEvents.OnSeasonChanged.RemoveListener(OnSeasonChanged);
    }
    
    private void OnHourPassed()
    {
        Debug.Log("An hour has passed!");
    }
    
    private void OnDayStarted()
    {
        Debug.Log("A new day has begun!");
    }
    
    private void OnSeasonChanged()
    {
        Debug.Log("The season has changed!");
    }
}
```

#### Option B: Implementing ITimeListener

```csharp
using StardewTimeSystem;
using UnityEngine;

public class MyCustomListener : TimeListenerBase
{
    public override void OnHourPassed(int currentHour)
    {
        Debug.Log($"Hour passed: {currentHour}");
    }
    
    public override void OnDayStarted(int day, Season season, int year)
    {
        Debug.Log($"Day {day} of {season} in year {year}");
    }
}
```

## 🎮 Usage Examples

### Accessing Current Time

```csharp
// Get current time components
int hour = TimeSystem.Instance.CurrentHour;
int minute = TimeSystem.Instance.CurrentMinute;
int day = TimeSystem.Instance.CurrentDay;
Season season = TimeSystem.Instance.CurrentSeason;
int year = TimeSystem.Instance.CurrentYear;

// Check if it's daytime
bool isDaytime = TimeSystem.Instance.IsDaytime;

// Get current time of day
TimeOfDay timeOfDay = TimeSystem.Instance.CurrentTimeOfDay;

// Get formatted strings
string formattedTime = TimeSystem.Instance.FormattedTime;      // "6:30 AM"
string formattedDate = TimeSystem.Instance.FormattedDate;      // "Spring 1, Year 1"
string formattedDateTime = TimeSystem.Instance.FormattedDateTime; // "Spring 1, Year 1 - 6:30 AM"
```

### Controlling Time

```csharp
// Pause/resume time
TimeSystem.Instance.Pause();
TimeSystem.Instance.Resume();
TimeSystem.Instance.TogglePause();

// Set specific time
TimeSystem.Instance.SetTime(14, 30); // 2:30 PM

// Set specific date
TimeSystem.Instance.SetDate(15, Season.Summer, 2); // Day 15 of Summer, Year 2

// Advance time
TimeSystem.Instance.AdvanceMinutes(30); // Advance 30 minutes
TimeSystem.Instance.AdvanceHours(2);    // Advance 2 hours
TimeSystem.Instance.AdvanceDays(7);     // Advance 7 days

// Reset to initial state
TimeSystem.Instance.Reset();
```

### Getting Progress Values

```csharp
// Get progress through the current day (0-1)
float dayProgress = TimeSystem.Instance.GetDayProgress();

// Get progress through the current season (0-1)
float seasonProgress = TimeSystem.Instance.GetSeasonProgress();

// Get progress through the current year (0-1)
float yearProgress = TimeSystem.Instance.GetYearProgress();

// Get time until next hour/day
float secondsUntilNextHour = TimeSystem.Instance.GetSecondsUntilNextHour();
float secondsUntilNextDay = TimeSystem.Instance.GetSecondsUntilNextDay();
```

### Using TimeData Configuration

```csharp
// Access the TimeData configuration
TimeData timeData = TimeSystem.Instance.TimeData;

// Check if it's daytime for a specific hour
bool isDaytime = timeData.IsDaytime(10); // Is 10 AM daytime?

// Get TimeOfDay for a specific hour
TimeOfDay timeOfDay = timeData.GetTimeOfDay(14); // What time of day is 2 PM?

// Get computed values
int totalMinutesInDay = timeData.TotalMinutesInDay;
float minuteDuration = timeData.MinuteDurationSeconds;
int totalDaysInYear = timeData.TotalDaysInYear;
```

### Working with Enums

```csharp
// Get display strings
string seasonName = Season.Summer.ToDisplayString(); // "Summer"
string timeOfDayName = TimeOfDay.Afternoon.ToDisplayString(); // "Afternoon"

// Navigate seasons
Season nextSeason = Season.Spring.Next(); // Summer
Season previousSeason = Season.Winter.Previous(); // Fall

// Navigate time of day
TimeOfDay nextTimeOfDay = TimeOfDay.Morning.Next(); // Afternoon
```

## 🎨 Sample Scene

The package includes a sample scene at `Samples/BasicSetup` that demonstrates:
- Setting up the TimeSystem
- Creating and configuring TimeData
- Displaying time on a UI Text element
- Changing lighting based on time of day
- Responding to time events

To use the sample:
1. Import the sample from the Package Manager
2. Open the sample scene
3. Run the scene to see the time system in action

## 🔌 API Reference

### TimeSystem (MonoBehaviour Singleton)

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Instance` | `TimeSystem` | Singleton instance |
| `CurrentMinute` | `int` | Current minute (0-59) |
| `CurrentHour` | `int` | Current hour (0-23) |
| `CurrentDay` | `int` | Current day of season (1-28) |
| `CurrentSeason` | `Season` | Current season |
| `CurrentYear` | `int` | Current year |
| `IsDaytime` | `bool` | Whether it's currently daytime |
| `CurrentTimeOfDay` | `TimeOfDay` | Current time of day |
| `TotalDays` | `int` | Total days passed since start |
| `FormattedTime` | `string` | Time as "6:30 AM" |
| `FormattedDate` | `string` | Date as "Spring 1, Year 1" |
| `FormattedDateTime` | `string` | Full date/time string |
| `TimeData` | `TimeData` | The configuration data |
| `IsPaused` | `bool` | Whether the system is paused |

#### Events

| Event | Description |
|-------|-------------|
| `OnInitialized` | Triggered when TimeSystem is initialized |
| `OnDestroyed` | Triggered when TimeSystem is destroyed |

#### Methods

| Method | Description |
|--------|-------------|
| `Pause()` | Pauses the time system |
| `Resume()` | Resumes the time system |
| `TogglePause()` | Toggles pause state |
| `SetTime(int hour, int minute)` | Sets the time to a specific hour and minute |
| `SetDate(int day, Season season, int year)` | Sets the date to specific values |
| `AdvanceMinutes(int minutes)` | Advances time by minutes |
| `AdvanceHours(int hours)` | Advances time by hours |
| `AdvanceDays(int days)` | Advances time by days |
| `Reset()` | Resets to initial state |
| `GetDayProgress()` | Gets progress through current day (0-1) |
| `GetSeasonProgress()` | Gets progress through current season (0-1) |
| `GetYearProgress()` | Gets progress through current year (0-1) |
| `GetSecondsUntilNextHour()` | Gets seconds until next hour |
| `GetSecondsUntilNextDay()` | Gets seconds until next day |
| `RegisterListener(ITimeListener)` | Registers a time listener |
| `UnregisterListener(ITimeListener)` | Unregisters a time listener |

### TimeData (ScriptableObject)

Configuration for the time system. Create via **Create > Stardew > Time System > Time Data**.

#### Time Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DayDurationSeconds` | `float` | 120 | Duration of a full day in real-world seconds |
| `HoursInDay` | `int` | 24 | Number of hours in a day |
| `MinutesInHour` | `int` | 60 | Number of minutes in an hour |

#### Day/Night Cycle

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DayStartHour` | `int` | 6 | Hour when day starts (sunrise) |
| `NightStartHour` | `int` | 18 | Hour when night starts (sunset) |

#### Time of Day Ranges

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DawnStartHour` | `int` | 0 | Hour when dawn starts |
| `DawnEndHour` | `int` | 6 | Hour when dawn ends |
| `MorningStartHour` | `int` | 6 | Hour when morning starts |
| `MorningEndHour` | `int` | 12 | Hour when morning ends |
| `AfternoonStartHour` | `int` | 12 | Hour when afternoon starts |
| `AfternoonEndHour` | `int` | 17 | Hour when afternoon ends |
| `EveningStartHour` | `int` | 17 | Hour when evening starts |
| `EveningEndHour` | `int` | 20 | Hour when evening ends |

#### Seasons

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DaysInSeason` | `int` | 28 | Number of days in each season |
| `SeasonsInYear` | `int` | 4 | Number of seasons in a year |

#### Time Scaling

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `TimeScale` | `float` | 1.0 | Multiplier for time speed |
| `PauseWhenNotFocused` | `bool` | true | Pause when game is not focused |

#### Start Time

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `StartYear` | `int` | 1 | Starting year |
| `StartSeason` | `int` | 0 | Starting season (0=Spring, 1=Summer, 2=Fall, 3=Winter) |
| `StartDay` | `int` | 1 | Starting day of season |
| `StartHour` | `int` | 6 | Starting hour |
| `StartMinute` | `int` | 0 | Starting minute |

#### Debug

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `EnableDebugLogs` | `bool` | false | Enable debug logging |

### TimeEvents (Static Class)

Event bus for time system notifications.

#### Events

| Event | Description |
|-------|-------------|
| `OnMinutePassed` | Invoked every in-game minute |
| `OnHourPassed` | Invoked every in-game hour |
| `OnDayStarted` | Invoked when a new day starts |
| `OnDayEnded` | Invoked when a day ends |
| `OnNightStarted` | Invoked when night starts |
| `OnNightEnded` | Invoked when night ends (day starts) |
| `OnSeasonChanged` | Invoked when the season changes |
| `OnYearChanged` | Invoked when the year changes |
| `OnDawnStarted` | Invoked when dawn starts |
| `OnMorningStarted` | Invoked when morning starts |
| `OnAfternoonStarted` | Invoked when afternoon starts |
| `OnEveningStarted` | Invoked when evening starts |
| `OnNightTimeStarted` | Invoked when night time starts |
| `OnSpecificHourReached` | Invoked when a specific hour is reached (passes hour as int) |
| `OnSpringStarted` | Invoked when spring starts |
| `OnSummerStarted` | Invoked when summer starts |
| `OnFallStarted` | Invoked when fall starts |
| `OnWinterStarted` | Invoked when winter starts |

#### Methods

| Method | Description |
|--------|-------------|
| `ClearAllEvents()` | Removes all event listeners |

### ITimeListener (Interface)

Interface for receiving time update callbacks.

#### Methods

| Method | Description |
|--------|-------------|
| `OnMinutePassed(int hour, int minute)` | Called every minute |
| `OnHourPassed(int hour)` | Called every hour |
| `OnDayStarted(int day, Season season, int year)` | Called when a new day starts |
| `OnDayEnded(int day, Season season, int year)` | Called when a day ends |
| `OnSeasonChanged(Season newSeason, int year)` | Called when season changes |
| `OnYearChanged(int newYear)` | Called when year changes |

### TimeListenerBase (Abstract Class)

Base implementation of `ITimeListener` for easier use. Override only the methods you need.

## 🎯 Best Practices

1. **Singleton Access**: Use `TimeSystem.Instance` to access the time system from anywhere in your code.

2. **Event Subscription**: Always unsubscribe from events in `OnDisable()` or `OnDestroy()` to prevent memory leaks.

3. **TimeData Configuration**: Create a `TimeData` asset and assign it to the `TimeSystem` component. This allows designers to tweak values without code changes.

4. **Performance**: The time system is optimized to only trigger events when state changes occur. However, avoid expensive operations in event handlers.

5. **Testing**: Use `TimeSystem.Instance.SetTime()` and `AdvanceMinutes()` methods in tests to control time precisely.

6. **Debugging**: Enable `EnableDebugLogs` in `TimeData` to see detailed time system information in the console.

## 🐛 Troubleshooting

### Time not advancing

- Check if `IsPaused` is true
- Verify that `TimeData.PauseWhenNotFocused` is set appropriately
- Ensure the GameObject with `TimeSystem` is not disabled

### Events not firing

- Verify you're subscribed to the correct events
- Check that you're not unsubscribing too early
- Use `TimeSystem.Instance.OnInitialized` to ensure TimeSystem is ready before subscribing

### TimeData not loading

- Ensure the `TimeData` asset is in a `Resources` folder if you want it loaded automatically
- Or assign it manually in the Inspector

### Incorrect time of day

- Check the `TimeOfDay` ranges in your `TimeData` configuration
- Verify the `DayStartHour` and `NightStartHour` values

## 📜 License

This package is licensed under the **MIT License**. See [LICENSE](LICENSE) for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## 📬 Support

For support, please open an issue on the repository.

---

**Stardew Time System** - A flexible time management system for Unity games.
