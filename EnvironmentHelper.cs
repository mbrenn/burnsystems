//-----------------------------------------------------------------------
// <copyright file="EnvironmentHelper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This helper class offers some method to determine environment information
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Stores, whether the environment is currently running under mono
        /// </summary>
        private static bool? isMono;

        /// <summary>
        /// Gets a value indicating whether the environment is currently running
        /// under mono
        /// </summary>
        public static bool IsMono
        {
            get
            {
                if (isMono.HasValue)
                {
                    return isMono.Value;
                }

                // Gets the type, only implemented by mono
                var t = Type.GetType("Mono.Runtime");
                isMono = (t != null);

                return isMono.Value;
            }
        }
    }
}
