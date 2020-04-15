using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_MSAL.Models;

namespace Test_MSAL.Services
{
    public interface IWeatherForcastService
    {
            Task<IEnumerable<WeatherForecast>> GetAsync();


    }
}
