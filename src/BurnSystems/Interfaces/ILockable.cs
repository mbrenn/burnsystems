
namespace BurnSystems.Interfaces
{
    /// <summary>
    /// This interface is used for all objects, which should 
    /// be locked by <c>SimpleLock</c>.
    /// </summary>
    public interface ILockable 
    {
        /// <summary>
        /// Locks an object
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlocks an object
        /// </summary>
        void Unlock();
    }
}
