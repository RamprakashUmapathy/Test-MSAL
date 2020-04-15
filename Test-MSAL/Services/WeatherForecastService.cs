using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Test_MSAL.Models;

namespace Test_MSAL.Services
{
    public class WeatherForecastService : IWeatherForcastService
    {

        private readonly HttpClient _httpClient;
        private readonly string _scope = string.Empty;
        private readonly string _baseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;
        public WeatherForecastService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _scope = configuration["WebAPIs:MyFirstService.Scope"];
            _baseAddress = configuration["WebAPIs:MyFirstService.BaseAddress"];

        }
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{ _baseAddress}/WeatherForecast");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            IEnumerable<WeatherForecast> results = JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(content, options);
            return results;
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _scope });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
