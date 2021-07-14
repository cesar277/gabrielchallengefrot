
namespace Frotcom.Challenge.Queue
{
    /// <summary>
    /// Factory to create IQueueProcessor instances
    /// </summary>
    public class QueueProcessorFactory : IQueueProcessorFactory
    {
        /// <summary>
        /// Create instance of IQueueProcessor
        /// </summary>
       public IQueueProcessor Create(){
            return new QueueProcessor();
        }
    }
}
