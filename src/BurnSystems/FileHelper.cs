namespace BurnSystems
{
    /// <summary>
    /// This class contains several helper methods affecting the filesystem
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Copies the directory from source dir to destination dir.
        /// Source from https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        /// </summary>
        /// <param name="sourceDir">Source Directory from which the files shall be copied</param>
        /// <param name="destinationDir">Destination directory to which the files will be copied</param>
        /// <param name="recursive">Flag, if copying shall be recursive</param>
        /// <exception cref="DirectoryNotFoundException">Thrown, if Source Directory does not exist</exception>
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
            {
                Directory.CreateDirectory(sourceDir);
                dir = new DirectoryInfo(sourceDir);
            }

            // Cache directories before we start copying
            var dirs = dir.GetDirectories();

            // Create the destination directory
            if (Directory.Exists(destinationDir))
            {
                // DeleteDirectory(destinationDir);
            }
            else
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Get the files in the source directory and copy to the destination directory
            foreach (var file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (!recursive) 
                return;
            
            foreach (var subDir in dirs)
            {
                var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
        }
    }
}
