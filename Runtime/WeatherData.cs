namespace WeatherServices
{
    public struct WeatherData
    {
        public WeatherType Type;
        public double Temperature;
        public float Humidity;
        public float Pressure;
        public float Visibility;
        public double WindSpeed;
        public int WindDirection;
        public bool IsDay;

        public override string ToString()
        {
            return $"Weather: {Type} \n" +
                   $"Temperature: {Temperature} \n" +
                   $"Humidity: {Humidity} \n" +
                   $"Pressure: {Pressure} \n" +
                   $"Visibility: {Visibility} \n" +
                   $"WindSpeed: {WindSpeed} \n" +
                   $"WindDirection: {WindDirection} \n" +
                   $"IsDay: {IsDay}";
        }
    }

    public enum WeatherType
    {
        None,
        Clear,
        Clouds,
        Rain,
        Drizzle,
        Snow,
        Thunderstorm,
        Mist,
        Smoke,
        Haze,
        Dust,
        Fog,
        Sand,
        Ash,
        Squall,
        Tornado,
    }
}