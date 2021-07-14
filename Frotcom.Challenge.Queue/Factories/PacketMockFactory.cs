using Frotcom.Challenge.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frotcom.Challenge.Queue.Factories
{
    internal static class PacketMockFactory
    {
        private static Random random => new Random();

        internal static Packet GetRandomPacket()
        {
            var speed = random.NextDouble() * 100;
            var latitude = random.NextDouble() * 9;
            var longitude = random.NextDouble() * 9;
            var vehicleId = random.NextDouble() * 50;

            return new Packet((int)vehicleId, speed, latitude, longitude);
        }
    }
}
