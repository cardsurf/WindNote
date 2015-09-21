using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WindNote.Resources.Utilities
{
    static class ResourceLocalizer
    {
        private static string resourcesDirectory = "Resources";
        private static string imagesDirectory = "Images";

        private static string executablePath = AppDomain.CurrentDomain.BaseDirectory;
        private static string projectPath = Directory.GetParent(executablePath).Parent.Parent.FullName.ToString();
        private static string resourcesPath = projectPath + Path.DirectorySeparatorChar + resourcesDirectory;

        public static string getImagesPath()
        {
            String path = resourcesPath + Path.DirectorySeparatorChar + imagesDirectory + Path.DirectorySeparatorChar;
            return path;
        }

    }
}
