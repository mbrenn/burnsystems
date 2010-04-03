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
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using BurnSystems.Test;

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

        /// <summary>
        /// Gets the type by xml-Configuration.
        /// </summary>
        /// <remarks>
        /// The Xml-Node can have the following subnodes:
        /// <list type="bullet">
        /// <item>path: Path to assembly. Can be relative or absolute</item>
        /// <item>strongname: Strongname of the assembly which is used to query
        /// GAC or local filesystem</item>
        /// <item>fullname: Name of the type as a fullname</item>        
        /// </list>
        /// <para>Whether path or strongname has to be set. </para>
        /// <para>If assembly or type is not found, an <c>InvalidOperationException</c>
        /// will be thrown.</para>
        /// </remarks>
        /// <param name="xmlType">Xmlnode storing the configuration</param>
        /// <returns>Found type.</returns>
        public static Type GetType(XElement xmlType)
        {
            Ensure.IsNotNull(xmlType);

            var xmlPath = xmlType.Element("path");
            var xmlStrongname = xmlType.Element("strongname");
            var xmlFullname = xmlType.Element("fullname");

            if ((xmlPath == null && xmlStrongname == null)
                || (xmlPath != null && xmlStrongname != null))
            {
                throw new InvalidOperationException(
                    LocalizationBS.EnvironmentHelper_PathOrStrongname);
            }

            if (xmlFullname == null)
            {
                throw new InvalidOperationException(
                    "fullname == null");
            }

            // Loads the assembly
            Assembly assembly = null;

            if (xmlPath != null)
            {
                // Path
                var path = Path.GetFullPath(xmlPath.Value);
                assembly = GetOrLoadAssembly(path);
            }
            else
            {
                // Strong name
                var strongName = xmlStrongname.Value;
                assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName == strongName)
                    .FirstOrDefault();

                if (assembly == null)
                {
                    assembly = AppDomain.CurrentDomain.Load(strongName);
                }

                // Check if assembly is still null
                if (assembly == null)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            LocalizationBS.EnvironmentHelper_AssemblyNotFound,
                            strongName));
                }
            }

            // Assembly should not be null. 
            Ensure.IsNotNull(assembly);

            // Now, it's time to get the type in the assembly
            var type = assembly.GetType(xmlFullname.Value);
            if (type == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        LocalizationBS.EnvironmentHelper_TypeNotFound,
                        xmlFullname.Value));
            }

            return type;
        }
    }
}
