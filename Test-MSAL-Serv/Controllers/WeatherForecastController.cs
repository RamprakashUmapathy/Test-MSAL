using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;

namespace Test_MSAL_Serv.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //ValidateAppRole("");
            var rng = new Random();
            WeatherForecast[] results = Enumerable.Range(1, 55).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
            //_logger.LogInformation(string.Join(',', results.Select(f => f.Summary).ToArray()));
            return results;
        }
        private void ValidateAppRole(string appRole)
        {
            //
            // The `role` claim tells you what permissions the client application has in the service.
            // In this case, we look for a `role` value of `access_as_application`.
            var claims = HttpContext.User.Claims;
            //Claim roleClaim = ClaimsPrincipal.Current.FindFirst("roles");
            foreach (Claim claim in claims)
            {
                _logger.LogInformation("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value + "</br>");
            }
            //if (roleClaim == null || !roleClaim.Value.Split(' ').Contains(appRole))
            //{
            //    throw new UnauthorizedAccessException(message: $"The 'roles' claim does not contain '{appRole}' or was not found");

            //}
        }
    }
}
