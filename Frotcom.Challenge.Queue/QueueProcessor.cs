using Frotcom.Challenge.Data.Models;
using Frotcom.Challenge.Reverse.Geocoding;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Frotcom.Challenge.Queue
{
    /// <summary>
    /// The queue messages can be consumed using an implementation of this interface.
    /// Note that it must be registered using <see cref="QueueProcessorHost{T}"/>
    /// </summary>
    public class QueueProcessor : IQueueProcessor
    {
        //I planned on using a ConcurrentBag instead of a List, but netstandard2.0 dont have the necessary functions 
        private static readonly ConcurrentDictionary<int, List<Packet>> packetsByVehicle = new ConcurrentDictionary<int, List<Packet>>(10, 100);

        /// <summary>
        /// Receive a batch of <see cref="Packet"/>, with the minimum size defined on the <see cref="QueueProcessorHost{T}"/> ctor
        /// </summary>
        /// <param name="packets">Packets</param>
        public async Task Process(List<Packet> packets, CancellationToken cancellationToken)
        {
            ReverseGeocoding geocoding = new ReverseGeocoding();
            while (!cancellationToken.IsCancellationRequested)
            {
                List<Packet> portugalPackets = new List<Packet>();
                foreach (Packet packet in packets)
                {
                    Country contry = await geocoding.GetCountry(packet.Latitude, packet.Longitude);
                    if (contry == Country.Portugal)
                    {
                        portugalPackets.Add(packet);
                        List<Packet> list = packetsByVehicle.GetOrAdd(packet.VehicleId, new List<Packet>());
                        lock (list)
                        {
                            list.Add(packet);
                            if (list.Count >= 100)
                            {
                                sendPackages(list, packet.VehicleId);
                                list.Clear();
                            }
                        }
                    }
                }
                QueueTimedLog.Instance.Log(packets, portugalPackets);
            }
        }

        private void sendPackages(List<Packet> packets, int vehicleId)
        {

            using (var clientHandle = new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; } })
            using (var httpClient = new HttpClient(clientHandle))
            {

                // httpClient.BaseAddress = new Uri("ClientURL");
                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"CLIENT_AUTHENTIFICATION");
                // var responseMessage = await httpClient.PostAsync($"v2/vehicle/{vehicleId}/data", new StringContent(JsonSerializer.Serialize(packets)));
                // if (responseMessage.IsSuccessStatusCode)
                // {
                   //  Console.WriteLine($"Vehicle{vehicleId} sent {packets.Count} packets in Portugal");
                //}
                Console.WriteLine($"Vehicle{vehicleId} sent {packets.Count} packets in Portugal");
                Thread.Sleep(500);
            }
        }
    }
}
