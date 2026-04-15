using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Client.Test;

[TestClass]
[TestCategory("Integration")]
public sealed class WeatherIntegrationTests
{
    ServiceProvider? sp;
    IServiceScope? scope;

    [TestInitialize]
    public void Initialize()
    {
        var config = new ConfigurationBuilder();
        config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["WeatherApi:Url"] = "https://weather.googleapis.com",
        })
        .AddJsonFile("appsettings.test.json", optional: true);

        var sc = new ServiceCollection();
        sc.AddWeatherService(config.Build());

        sp = sc.BuildServiceProvider(true);
        scope = sp.CreateScope();
    }

    [TestCleanup]
    public void Cleanup()
    {
        scope?.Dispose();
        sp?.Dispose();
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetCurrentConditionsTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new CurrentConditionsRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
        };

        // Act
        var temperature = await weatherClient.GetCurrentConditionsAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetForecastDaysTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new ForecastDaysRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
            Days = 1,
            PageSize = 1,
        };

        // Act
        var temperature = await weatherClient.GetForecastDaysAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetForecastHoursTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new ForecastHoursRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
            Hours = 1,
            PageSize = 1,
        };

        // Act
        var temperature = await weatherClient.GetForecastHoursAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetHistoryHoursTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new HistoryHoursRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
            Hours = 1,
            PageSize = 1,
        };

        // Act
        var temperature = await weatherClient.GetHistoryHoursAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    [Ignore("Information is not supported for this location.")]
    public async Task GetPublicAlertsTest(double latitude, double longitude)
    {
        // Arrange
        var weatherClient = scope!.ServiceProvider.GetRequiredService<IWeatherClient>()!;

        var request = new PublicAlertsRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            PageSize = 1,
        };

        // Act
        var temperature = await weatherClient.GetPublicAlertsAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }
}
