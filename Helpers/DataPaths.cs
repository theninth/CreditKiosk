using System;
using System.IO;

namespace CreditKiosk.Helpers
{
    public static class DataPaths
    {
        /// <summary>
        /// Creates a folder in the users AppData\local folder with the AppName and returns it's path.
        /// </summary>
        /// <returns>Full path of the directory.</returns>
        /// <exception cref="Exception">When the AppData folder is'nt found.</exception>
        public static string CreateAndGetAppDataFolder()
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            string appName = (string)App.Current.Resources["AppName"];
            string? path = Environment.GetFolderPath(folder);

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Could not get folder path for AppData (or similar)");
            }

            string fullpath = Path.Join(path, appName);
            Directory.CreateDirectory(fullpath);
            return fullpath;
        }
    }
}
