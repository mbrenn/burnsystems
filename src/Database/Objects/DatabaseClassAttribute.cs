using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Database.Objects
{
    /// <summary>
    /// This attribute indicates that the class can be stored into database
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DatabaseClassAttribute : Attribute
    {
    }
}
