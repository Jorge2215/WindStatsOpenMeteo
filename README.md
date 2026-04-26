# WindStats – OpenMeteo

A real-time wind statistics dashboard built with **ASP.NET Core Blazor Server** (.NET 10) and the free [Open-Meteo](https://open-meteo.com/) weather API. No API key required.

---

## Features

- **Auto-detects your location** via the browser Geolocation API.
- **City search fallback** – if geolocation is denied or unavailable, search by city name using the Open-Meteo Geocoding API.
- **Live KPI cards** – current wind speed, direction (compass arrow + label), gusts, and Beaufort scale with description.
- **Daily summary** – max speed, average speed, and max gusts for the current day.
- **24-hour hourly forecast** table with speed, direction, and gusts; current hour is highlighted.
- **Interactive map** (Leaflet + OpenStreetMap) showing a 40 km coverage radius circle and a colour-coded wind-direction arrow at your location.
- **Reverse geocoding** via Nominatim (OpenStreetMap) to resolve coordinates into a human-readable city/country name.
- Manual **Refresh** button to re-fetch the latest data.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Blazor Server (.NET 10) |
| UI | Bootstrap 5, custom CSS |
| Map | Leaflet.js + OpenStreetMap tiles |
| Wind data | [Open-Meteo Forecast API](https://open-meteo.com/en/docs) |
| Geocoding (search) | [Open-Meteo Geocoding API](https://open-meteo.com/en/docs/geocoding-api) |
| Reverse geocoding | [Nominatim / OpenStreetMap](https://nominatim.org/) |
| JS interop | Browser Geolocation API, Leaflet map interop |

---

## Project Structure

```
WindStatsOpenMeteo/
├── Components/
│   ├── Pages/
│   │   └── Home.razor          # Main dashboard page
│   └── Layout/
│       ├── MainLayout.razor    # App shell
│       └── NavMenu.razor       # Navigation sidebar
├── Models/
│   ├── WindData.cs             # Current wind reading + Beaufort/direction helpers
│   ├── WindResult.cs           # Current + hourly readings, daily aggregates
│   ├── HourlyWindReading.cs    # Single hourly entry
│   ├── LocationInfo.cs         # Resolved location (lat/lon, city, country)
│   ├── OpenMeteoApiResponse.cs # Open-Meteo forecast API DTO
│   ├── GeocodingApiResponse.cs # Open-Meteo geocoding API DTO
│   ├── GeolocationResult.cs    # Browser geolocation JS interop DTO
│   └── NominatimResponse.cs    # Nominatim reverse-geocoding DTO
├── Services/
│   ├── OpenMeteoService.cs     # Fetches wind forecast data
│   └── GeocodingService.cs     # City search + reverse geocoding
├── wwwroot/
│   ├── app.css                 # Global styles and wind-card / compass styles
│   └── js/windApp.js           # JS interop: geolocation + Leaflet map
└── Program.cs                  # App bootstrap, DI, HTTP client registration
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run locally

```bash
git clone <repository-url>
cd WindStatsOpenMeteo
dotnet run
```

Then open your browser at:

- HTTP: `http://localhost:5221`
- HTTPS: `https://localhost:7069`

When prompted by the browser, **allow location access** to auto-populate your coordinates. If you deny it, a city-search box will appear.

---

## External APIs

All three external services are free and require no authentication:

| Service | Base URL | Purpose |
|---|---|---|
| Open-Meteo Forecast | `https://api.open-meteo.com/v1/` | Wind speed, direction, gusts (current + 24 h hourly) |
| Open-Meteo Geocoding | `https://geocoding-api.open-meteo.com/v1/` | City name → coordinates search |
| Nominatim | `https://nominatim.openstreetmap.org/` | Coordinates → city/country name |

HTTP clients are registered with named clients and per-service timeouts in `Program.cs`. The Nominatim client includes a `User-Agent` header as required by the Nominatim usage policy.

---

## Data & Units

- Wind speed and gusts: **km/h**
- Wind direction: **degrees** (0–360°), converted to a 16-point compass label (N, NNE, NE, …)
- Beaufort scale: computed from speed in km/h (0 = Calm → 12 = Hurricane)
- Forecast range: current day only (`forecast_days=1`), timezone resolved automatically by the API

---

## Configuration

All settings use ASP.NET Core defaults. No secrets or API keys are needed. The `appsettings.Development.json` file enables detailed logging in the development environment.

To change the launch port, edit `Properties/launchSettings.json`.
