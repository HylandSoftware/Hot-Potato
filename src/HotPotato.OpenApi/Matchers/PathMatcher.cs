using System.Collections.Generic;
using System.Linq;
using System;

namespace HotPotato.OpenApi.Matchers
{
	public static class PathMatcher
	{
		/// <summary>
		/// Finds the path in the spec that matches the path in the pair provided.
		/// </summary>
		/// <param name="pairPath"></param>
		/// <param name="specPaths"></param>
		/// <returns></returns>
		public static string Match(string pairPath, IEnumerable<string> specPaths)
		{
			if (specPaths.Contains(pairPath))
			{
				return pairPath;
			}

			string[] pairPathPieces = preparePathPieces(pairPath);

			foreach (string specPath in specPaths)
			{
				string[] specPathPieces = preparePathPieces(specPath);

				int i = 0;
				bool match = false;
				Stack<string> pathStack = new Stack<string>();

				while (i < specPathPieces.Length && i < pairPathPieces.Length)
				{
					if (isParam(specPathPieces[i]) || (specPathPieces[i] == pairPathPieces[i]))
					{
						pathStack.Push(specPathPieces[i]);
						if (pathStack.Count == specPathPieces.Length)
						{
							match = true;
						}
					}
					else
					{
						match = false;
						break;
					}
					i++;
				}

				if (match)
				{
					string matchedPath = $"/{string.Join('/', pathStack)}";
					return matchedPath;
				}
			}
			return null;
		}

		private static bool isParam(string s)
		{
			return s.StartsWith('{') && s.EndsWith('}');
		}

		/// <summary>
		/// Split paths by '/' to match pieces of the pair's path to parameters in the spec's path
		/// </summary>
		private static string[] preparePathPieces(string path)
		{
			string[] pathPieces = path.Split('/');
			pathPieces = pathPieces.Where(x => !string.IsNullOrEmpty(x)).ToArray();
			//Reverse here since some server paths have more than one piece, e.g. https://api.hyland.com/ibpaf/rdds
			//In cases like this, the relative path in the request can be /ibpaf/rdds/messages/78,
			//but will need to be matched with '/messages/{messageId}'
			Array.Reverse(pathPieces);
			return pathPieces;
		}
	}
}
