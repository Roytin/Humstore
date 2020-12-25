using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntX.Data
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static WeatherForecast[] _sourceData;
        private static WeatherForecast[] SourceData
        {
            get 
            {
                if (_sourceData == null)
                {
                    var rng = new Random();
                    _sourceData = Enumerable.Range(0, 100).Select(index => new WeatherForecast
                    {
                        Id = index.ToString(),
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)],
                    }).ToArray();
                }
                return _sourceData;
            }
        }

        public async Task<WeatherForecast[]> GetForecastAsync(int page, int limit)
        {
            await Task.Delay(100);
            return SourceData.Skip((page - 1) * limit).Take(limit).ToArray();
        }
    }
}
