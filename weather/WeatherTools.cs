using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace QuickstartWeatherServer.Tools;

[McpServerToolType]
public static class WeatherTools
{
    [McpServerTool, Description("Get the latitude and longitude for a city name.")]
    public static async Task<string> GetLocation(
        HttpClient client,
        [Description("The name of the city (e.g., 'Kolkata', 'London').")] string city)
    {
        // Open-Meteo Geocoding API
        var url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=en&format=json";
        
        using var jsonDocument = await client.ReadJsonDocumentAsync(url);
        var root = jsonDocument.RootElement;

        if (!root.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
        {
            return $"No location found for '{city}'.";
        }

        var location = results[0];
        var name = location.GetProperty("name").GetString();
        var country = location.TryGetProperty("country", out var c) ? c.GetString() : "Unknown";
        var lat = location.GetProperty("latitude").GetDouble();
        var lon = location.GetProperty("longitude").GetDouble();

        return $"""
                Found: {name}, {country}
                Latitude: {lat}
                Longitude: {lon}
                """;
    }

    [McpServerTool, Description("Get the current weather forecast for a location (latitude/longitude).")]
    public static async Task<string> GetForecast(
        HttpClient client,
        [Description("Latitude of the location.")] double latitude,
        [Description("Longitude of the location.")] double longitude)
    {
        // Open-Meteo Weather API
        var url = string.Create(CultureInfo.InvariantCulture, 
            $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");

        using var jsonDocument = await client.ReadJsonDocumentAsync(url);
        var root = jsonDocument.RootElement;
        
        if (!root.TryGetProperty("current", out var current))
        {
             return "Could not retrieve current weather data.";
        }

        var temp = current.GetProperty("temperature_2m").GetDouble();
        var wind = current.GetProperty("wind_speed_10m").GetDouble();
        var time = current.GetProperty("time").GetString();

        var currentUnits = root.GetProperty("current_units");
        var tempUnit = currentUnits.GetProperty("temperature_2m").GetString();
        var windUnit = currentUnits.GetProperty("wind_speed_10m").GetString();

        return $"""
                Time: {time}
                Temperature: {temp} {tempUnit}
                Wind Speed: {wind} {windUnit}
                (Source: Open-Meteo)
                """;
    }
}
