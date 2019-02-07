using System.Collections.Generic;
using System.Linq;
using System;

namespace HotPotato.Matchers
{
    public class PathMatcher
    {
        /// <summary>
        /// Finds the path in <paramref name="paths"/> that matches the <paramref name="path"/> provided.
        /// </summary>
        /// <param name="reqPath"></param>
        /// <param name="specPaths"></param>
        /// <returns></returns>
        public static string Match(string reqPath, IEnumerable<string> specPaths)
        {
            string[] reqPathPieces = preparePathPieces(reqPath);

            foreach (string specPath in specPaths)
            {
                string[] specPathPieces = preparePathPieces(specPath);

                int i = 0;
                bool match = true;
                Stack<string> pathStack = new Stack<string>();

                while (i < specPathPieces.Length && i < reqPathPieces.Length)
                {
                    if (isParam(specPathPieces[i]) || (specPathPieces[i] == reqPathPieces[i]))
                    {
                        pathStack.Push(specPathPieces[i]);
                    }
                    else
                    {
                        match = false;
                        break;
                    }
                    i++;
                }

                if (match == true)
                {
                    string retString = "/" + string.Join('/', pathStack);
                    return retString;
                }
            }
            return string.Empty;
        }

        private static bool isParam(string s)
        {
            return s.StartsWith('{') && s.EndsWith('}');
        }

        private static string[] preparePathPieces(string path)
        {
            string[] pathPieces = path.Split('/');
            pathPieces = pathPieces.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Array.Reverse(pathPieces);
            return pathPieces;
        }
    }
}
