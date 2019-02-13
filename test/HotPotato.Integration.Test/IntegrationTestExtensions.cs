using System;
using System.IO;

namespace HotPotato
{
    public static class IntegrationTestExtensions
    {
        /// <summary>
        /// Returns a path with the correct directory separator based on your platform
        /// </summary>
        public static string ToPlatformPath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
