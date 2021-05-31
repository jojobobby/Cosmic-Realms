using System;
using wServer.realm.worlds;

namespace wServer.realm
{
    public class WeatherManager
    {
        private readonly World target;
        private readonly Random rand;

        public WeatherManager(World target)
        {
            this.target = target;
            this.rand = new Random();
        }

        public void Tick(RealmTime time)
        {
            if (time.TickDelta % 20 == 0)
            {
                rand.Next();
            }
            if ((time.TickDelta) % 5000 == 0)
            {
                target.ChangeWeather(randomWeather());
            }
        }

        private Weather randomWeather()
        {
            Weather w;
            do w = (Weather)Enum.Parse(typeof(Weather), Enum.GetName(typeof(Weather), rand.Next(Enum.GetValues(typeof(Weather)).Length - 1)));
            while (w == target.Weather);
            return w;
        }
    }
}
