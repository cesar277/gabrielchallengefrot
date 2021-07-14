using Frotcom.Challenge.Data.Models;
using Frotcom.Challenge.Data.Models;
using Frotcom.Challenge.Reverse.Geocoding;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Frotcom.Challenge.Queue
{
    public class QueueTimedLog
    {
        private const int QueueTimer = 10000;
        private int totalPacketsCount = 0;
        private int portugalPacketsCount = 0;
        private static readonly QueueTimedLog instance = new QueueTimedLog();

        public static QueueTimedLog Instance
        {
            get
            {
                return instance;
            }
        }

        public void startLog(CancellationTokenSource cancellationToken){
            Task.Run(() => {
                TimedLog(cancellationToken);
            });
        }

        public void Log(List<Packet> packets, List<Packet> portugalPackets)
        {
            totalPacketsCount += packets.Count;
            portugalPacketsCount += portugalPackets.Count;
        }

        private void TimedLog(CancellationTokenSource cancellationToken){
            while(!cancellationToken.Token.IsCancellationRequested)
            {
                Console.WriteLine($"Total: {totalPacketsCount}, InPortugal: {portugalPacketsCount}");
                totalPacketsCount = 0;
                portugalPacketsCount = 0;
                Thread.Sleep(QueueTimer);
            }
        }
    }
}
