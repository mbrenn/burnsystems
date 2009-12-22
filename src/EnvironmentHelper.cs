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
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// This helper class offers some method to determine environment information
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Stores, whether the environment is currently running under mono
        /// </summary>
        private static readonly bool isMono;

        /// <summary>
        /// Initializes static members of the EnvironmentHelper class.
        /// </summary>
        static EnvironmentHelper()
        {
            // Gets the type, only implemented by mono
            var t = Type.GetType("Mono.Runtime");
            isMono = t != null;
        }

        /// <summary>
        /// Gets a value indicating whether the environment is currently running
        /// under mono
        /// </summary>
        public static bool IsMono
        {
            get
            {
                return isMono;
            }
        }
        
        /// <summary>
        /// Gets or loads an assembly. If assembly is already loaded by process, 
        /// it will be directly returned. If it is not available, it will be loaded
        /// by <c>Assembly.LoadFile</c>.
        /// </summary>
        /// <param name="assemblyPath">Path to assembly</param>
        /// <returns>Loaded or already loaded assembly.</returns>
        public static Assembly GetOrLoadAssembly(string assemblyPath)
        {
            Assembly assembly = null;

            // Checks whether the assembly has already been loaded
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var loadedAssembly in currentAssemblies)
            {
                var fullPath = Path.GetFullPath(loadedAssembly.Location);
                if (Path.GetFileName(fullPath) == Path.GetFileName(assemblyPath))
                {
                    assembly = loadedAssembly;
                    break;
                }
            }

            // Loads assembly
            if (assembly == null)
            {
                assembly = Assembly.LoadFile(assemblyPath);
            }

            if (assembly == null)
            {
                // Assembly not found
                throw new InvalidOperationException(
                    String.Format(
                        LocalizationBS.EnvironmentHelper_AssemblyNotFound, assemblyPath));
            }

            return assembly;
        }
    }
}
