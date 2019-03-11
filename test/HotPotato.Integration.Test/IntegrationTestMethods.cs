using System;
using System.IO;

namespace HotPotato
{
    public static class IntegrationTestMethods
    {
        /// <summary>
        /// Returns a path with the correct directory separator based on your platform
        /// </summary>
        public static string SpecPath(string subPath, string file)
        {
            string path = Path.Combine(Environment.CurrentDirectory, subPath, file);
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
