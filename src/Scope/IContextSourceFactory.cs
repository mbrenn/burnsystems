
namespace BurnSystems.Scope
{
    /// <summary>
    /// All objects, which are capable to create a context source may implement this 
    /// interface. 
    /// This interface can be used by components, which would like to extend
    /// their context by querying of their context source
    /// </summary>
    public interface IContextSourceFactory
    {
        /// <summary>
        /// Creates the context source
        /// </summary>
        /// <returns>Created contextsource</returns>
        IContextSource CreateContextSource();
    }
}
