using Frotcom.Challenge.Queue.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Frotcom.Challenge.Queue
{
    /// <summary>
    /// This class is responsible for registering one or more <see cref="IQueueProcessor"/> instances, in order to consume mocked vehicles tracking data.
    /// The number of total instances is defined with the parameter totalQueueListener of the ctor.
    /// Note that those instances will consume from the queue in parallel.
    /// The minimum batch size can also be defined with the parameter batchSize of the ctor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueProcessorHost
    {
        private readonly int _batchSize;
        private readonly IQueueProcessorFactory _factory;
        private readonly int _totalQueueListeners;
        private readonly CancellationTokenSource _cancellationToken;

        /// <summary>
        /// This class is responsible for registering one or more <see cref="IQueueProcessor"/> instances.
        /// The number of total instances is defined with the parameter totalQueueListener of the ctor.
        /// Note that those instances will consume from the queue in parallel.
        /// The minimum batch size can also be defined with the parameter batchSize of the ctor.
        /// </summary>
        /// <param name="totalQueueListeners">Total <see cref="IQueueProcessor"/> instances</param>
        /// <param name="batchSize">Minimum batch size of packets received in <see cref="IQueueProcessor.Process(IEnumerable{Data.Models.Packet})"/></param>
        public QueueProcessorHost(IQueueProcessorFactory factory, CancellationTokenSource cancellationToken, int totalQueueListeners, int batchSize)
        {
            _batchSize = batchSize;
            _factory = factory;
            _totalQueueListeners = totalQueueListeners;
            _cancellationToken = cancellationToken;
        }

        private IQueueProcessor[] CreateInstances() => Enumerable
            .Range(0, _totalQueueListeners)
            .Select(_ => _factory.Create())
            .ToArray();

        /// <summary>
        /// Register the <see cref="IQueueProcessor"/> instances and start processing.
        /// </summary>
        public async Task Run()
        {
            var instances = CreateInstances();

            while(!_cancellationToken.Token.IsCancellationRequested)
            {
                await Task.WhenAll(instances.Select(instance =>
                {
                    var packets = Enumerable.Range(1, _batchSize).Select(_ => PacketMockFactory.GetRandomPacket()).ToList();
                    return instance.Process(packets, _cancellationToken.Token);
                }));
                await Task.Delay(500);
            }
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }
    }
}
