
using System.IO;

namespace HotPotato
{
    public static class IntegrationTestMethods
    {
        /// <summary>
        /// Returns a path with the correct directory separator based on your platform
        /// </summary>
        public static string SpecPath(string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
