using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HotPotato.Matchers
{
    public class PathMatcher
    {
        /// <summary>
        /// Finds the path in <paramref name="paths"/> that matches the <paramref name="path"/> provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Match(string path, IEnumerable<string> paths)
        {
            string[] pathPieces = path.Split('/');
            //var filteredPaths = paths.Where((p) => p.Split('/').Length == pathPieces.Length);
            foreach (var item in paths)
            {
                string[] checkPath = item.Split('/');
                if (checkPath.Length != pathPieces.Length)
                {
                    continue;
                }
                bool match = false;
                for (int i = 0; i < pathPieces.Length; i++)
                {
                    if (checkPath[i] != pathPieces[i])
                    {
                        if (isParam(checkPath[i]))
                        {
                            if (i == pathPieces.Length - 1)
                            {
                                match = true;
                            }
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        if (i == pathPieces.Length - 1)
                        {
                            match = true;
                        }
                    }
                }
                if (match)
                {
                    return item;
                }
            }
            return string.Empty;
        }

        private static bool isParam(string s)
        {
            return s.StartsWith('{') && s.EndsWith('}');
        }
    }
}
