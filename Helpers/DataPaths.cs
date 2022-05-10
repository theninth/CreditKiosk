using System;
using System.IO;

namespace CreditKiosk.Helpers
{
    public static class DataPaths
    {
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
