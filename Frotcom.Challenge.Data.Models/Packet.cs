using System;

namespace Frotcom.Challenge.Data.Models
{
    /// <summary>
    /// Data model for the device communication
    /// </summary>
    public class Packet
    {
        public Packet(int vehicleId, double speed, double latitude, double longitude)
        {
            VehicleId = vehicleId;
            Speed = speed;
            Latitude = latitude;
            Longitude = longitude;
        }

        public int VehicleId { get; private set; }
        public double Speed { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
    }
}
