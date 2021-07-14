using Frotcom.Challenge.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Frotcom.Challenge.Queue
{
    /// <summary>
    /// The queue messages can be consumed using an implementation of this interface.
    /// Note that it must be registered using <see cref="QueueProcessorHost{T}"/>
    /// </summary>
    public interface IQueueProcessor
    {
        /// <summary>
        /// Receive a batch of <see cref="Packet"/>, with the minimum size defined on the <see cref="QueueProcessorHost{T}"/> ctor
        /// </summary>
        /// <param name="packets">Packets</param>
        Task Process(List<Packet> packets, CancellationToken cancellationToken);
    }
}
