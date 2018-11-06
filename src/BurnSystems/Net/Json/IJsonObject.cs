namespace BurnSystems.Net.Json
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This interface has to be implemented by all objects that can
    /// be used as json object. 
    /// The interface has no properties or methods, it is only used for 
    /// declaration of objects for conversion.
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public interface IJsonObject
    {
    }
}
