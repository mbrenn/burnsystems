namespace BurnSystems.Collections
{

    /// <summary>
    /// This interface has to be implemented by all objects, having a certain
    /// key-object and wants to be used the advantages of the autodictionary
    /// for example.
    /// </summary>
    public interface IHasId
    {
        /// <summary>
        /// Gets the name of the key of the instance
        /// </summary>
        long Id
        {
            get;
        }
    }
}
