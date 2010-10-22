//-----------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Martin Brenn">
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
    using System.IO;

    /// <summary>
    /// This class contains several helper methods affecting the filesystem
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Copies a complete directory from one place to another. 
        /// If necessary, the directories are created
        /// </summary>
        /// <param name="sourcePath">Path containing the source</param>
        /// <param name="targetPath">Path containing the target</param>
        static public void CopyDirectory(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            foreach (var directory in Directory.GetDirectories(sourcePath))
            {
                var newSourcePath = Path.Combine(sourcePath, directory);
                var newTargetPath = Path.Combine(targetPath, directory);
                // Copy recursively
                CopyDirectory(newSourcePath, newTargetPath);
            }

            foreach (var file in Directory.GetFiles(sourcePath))
            {
                var newSourcePath = Path.Combine(sourcePath, file);
                var newTargetPath = Path.Combine(targetPath, file);

                File.Copy(newSourcePath, newTargetPath);
            }
        }
    }
}
