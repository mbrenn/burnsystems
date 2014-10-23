using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.License
{
    /// <summary>
    /// Defines that components, implementing the given interface, does not to satisfy 
    /// the GPL licence, even if the product itself is under GPL licence.
    /// This attribute does not have any other purpose and will not affect the 
    /// runtime
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class NonGPLInterfaceAttribute : Attribute
    {
    }
}
