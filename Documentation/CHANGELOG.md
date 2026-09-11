# Changelog

All notable changes to the **Stardew Time System** package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] - 2024-01-00

### Added

- Initial release of Stardew Time System package
- Core `TimeSystem` MonoBehaviour singleton
- `TimeData` ScriptableObject for configuration
- `TimeEvents` static event bus with comprehensive events
- `Season` and `TimeOfDay` enums with extension methods
- `ITimeListener` interface and `TimeListenerBase` abstract class
- Assembly definition file for proper compilation
- Comprehensive documentation (README, LICENSE, CHANGELOG)
- Sample scene demonstrating basic usage

### Features

- Configurable day duration (real-world seconds)
- 24-hour clock with minutes
- Day/Night cycle detection
- Season system (Spring, Summer, Fall, Winter)
- Year tracking
- Time of Day detection (Dawn, Morning, Afternoon, Evening, Night)
- Event-based notifications for all time changes
- Direct callback interface for time updates
- Pause/resume functionality
- Time scaling for debugging
- Time manipulation methods (SetTime, SetDate, AdvanceMinutes, etc.)
- Progress calculation methods (GetDayProgress, GetSeasonProgress, etc.)
- Formatted string outputs (FormattedTime, FormattedDate, FormattedDateTime)

---

## [Unreleased]

### Added
- (Future features will be listed here)

### Changed
- (Future changes will be listed here)

### Fixed
- (Future bug fixes will be listed here)

---

## Template for Future Entries

```
## [X.Y.Z] - YYYY-MM-DD

### Added
- Feature 1
- Feature 2

### Changed
- Change 1
- Change 2

### Fixed
- Bug fix 1
- Bug fix 2

### Removed
- Deprecated feature 1
- Deprecated feature 2
```
