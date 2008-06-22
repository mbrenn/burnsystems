//-----------------------------------------------------------------------
// <copyright file="IFactory.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// Dieses Interface ist eine Fabrik für eine bestimmte Art
    /// von Objekten
    /// </summary>
    public delegate T FactoryDelegate<T>();

    /// <summary>
    /// Dieser Delegat lüsst eine neue Instanz mit dem dahinterliegenden
    /// Typ TResult erzeugen und übergibt den Parameter vom Typ TParameter. 
    /// </summary>
    /// <typeparam name="TResult">Typ des neuerzeugten Objektes</typeparam>
    /// <typeparam name="TParameter">Typ der Parameter</typeparam>
    /// <param name="oParameter">übergebene Parameter</param>
    /// <returns>Neu erzeugtes Objekt</returns>
    public delegate TResult FactoryDelegate<TResult, TParameter> ( TParameter oParameter );
    
}
