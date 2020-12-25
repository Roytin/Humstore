using System;

namespace AntX.Data
{
    public class WeatherForecast
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    //public class WeatherForecast
    //{
    //    public int Id { get; set; }

    //    //[DisplayName("Date")]
    //    public DateTime Date { get; set; }

    //    //[DisplayName("Temp. (C)")]
    //    public int TemperatureC { get; set; }

    //    //[DisplayName("Summary")]
    //    public string Summary { get; set; }

    //    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    //    public bool Hot { get; set; }
    //}
}
