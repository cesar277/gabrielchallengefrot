using System;
using System.Threading.Tasks;

namespace Frotcom.Challenge.Reverse.Geocoding
{
    /// <summary>
    /// This class can be used to get the location for a given Latitude and Longitude
    /// </summary>
    public class ReverseGeocoding
    {
        private static Country[] allCountries = (Country[])Enum.GetValues(typeof(Country));
        private static Random random = new Random();

        /// <summary>
        /// Return the country of the latitude and longitude received
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <returns><see cref="Country"/></returns>
        public Task<Country> GetCountry(double latitude, double longitude)
        {
            var index = random.Next(0, allCountries.Length - 1);
            return Task.FromResult(allCountries[index]);
        }
    }
}
