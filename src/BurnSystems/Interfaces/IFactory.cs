namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Dieses Interface ist eine Fabrik für eine bestimmte Art
    /// von Objekten
    /// </summary>
    /// <typeparam name="T">Type of object to be created in factory. </typeparam>
    /// <returns>Created object by factory</returns>
    [Obsolete]
    public delegate T Factory<T>();

    /// <summary>
    /// Dieser Delegat lüsst eine neue Instanz mit dem dahinterliegenden
    /// Typ TResult erzeugen und übergibt den Parameter vom Typ TParameter. 
    /// </summary>
    /// <typeparam name="TResult">Typ des neuerzeugten Objektes</typeparam>
    /// <typeparam name="TParameter">Typ der Parameter</typeparam>
    /// <param name="parameter">übergebene Parameter</param>
    /// <returns>Neu erzeugtes Objekt</returns>
    [Obsolete]
    public delegate TResult Factory<TResult, TParameter>(TParameter parameter);    
}
