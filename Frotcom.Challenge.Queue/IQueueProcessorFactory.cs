
namespace Frotcom.Challenge.Queue
{
    /// <summary>
    /// Factory to create IQueueProcessor instances
    /// </summary>
    public interface IQueueProcessorFactory
    {
        /// <summary>
        /// Create instance of IQueueProcessor
        /// </summary>
        IQueueProcessor Create();
    }
}
