using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;

namespace Weather.Client.Test;

[TestClass]
[TestCategory("Unit")]
public sealed class WeatherUnitTests
{
    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetCurrentConditionsTest(double latitude, double longitude)
    {
        // Arrange
        using var httpClient = CreateHttpClientMoq(new CurrentConditionsResponse());
        var weatherClient = new WeatherClient(httpClient);

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
        using var httpClient = CreateHttpClientMoq(new ForecastDaysResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new ForecastDaysRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
            Days = 1,
            PageSize = 24,
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
        using var httpClient = CreateHttpClientMoq(new ForecastHoursResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new ForecastHoursRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
            Hours = 1,
            PageSize = 24,
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
        using var httpClient = CreateHttpClientMoq(new HistoryHoursResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new HistoryHoursRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            UnitsSystem = UnitsSystem.METRIC,
            Hours = 1,
            PageSize = 24,
        };

        // Act
        var temperature = await weatherClient.GetHistoryHoursAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }

    [TestMethod]
    [DataRow(55.7569, 37.6151)]
    public async Task GetPublicAlertsTest(double latitude, double longitude)
    {
        // Arrange
        using var httpClient = CreateHttpClientMoq(new PublicAlertsResponse());
        var weatherClient = new WeatherClient(httpClient);

        var request = new PublicAlertsRequest
        {
            LocationLatitude = latitude,
            LocationLongitude = longitude,
            LanguageCode = LanguageCode.Ru,
            PageSize = 24,
        };

        // Act
        var temperature = await weatherClient.GetPublicAlertsAsync(request, default);

        // Assets
        Assert.IsNotNull(temperature);
    }

    private static HttpClient CreateHttpClientMoq(object result)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = JsonContent.Create(result),
           });

        var client = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://test.com") };
        return client;
    }
}
