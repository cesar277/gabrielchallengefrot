using Frotcom.Challenge.Data.Models;
using Frotcom.Challenge.Queue;
using Frotcom.Challenge.Reverse.Geocoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Frotcom.Challenge.SendTrackingDataWorker
{
    class Program
    {
        /// <summary>
        /// FROTCOM CHALLENGE STARTS HERE
        /// </summary>
        public static void Main()
        {
            while(true){
                Console.WriteLine("Press any key to stop...");
                IQueueProcessorFactory factory = new QueueProcessorFactory();
                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                QueueProcessorHost queue = new QueueProcessorHost(factory, cancellationToken, 1, 100);
                Task.Run(async () => {
                    await queue.Run();
                });
                QueueTimedLog.Instance.startLog(cancellationToken);
                Console.ReadKey();
                cancellationToken.Cancel();
                Console.WriteLine("Press any key to start...");
                Console.ReadKey();
            }
        }
    }
}
