using System.IO;

namespace BurnSystems
{
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
        /// <param name="doOverwrite">Flag, if file shall be overwritten</param>
        public static void CopyDirectory(string sourcePath, string targetPath, bool doOverwrite = false)
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
                CopyDirectory(newSourcePath, newTargetPath, doOverwrite);
            }

            foreach (var file in Directory.GetFiles(sourcePath))
            {
                var newSourcePath = Path.Combine(sourcePath, file);
                var newTargetPath = Path.Combine(targetPath, Path.GetFileName(file));

                File.Copy(newSourcePath, newTargetPath, doOverwrite);
            }
        }
    }
}
