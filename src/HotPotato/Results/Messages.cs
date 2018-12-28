using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Results
{
    public static class Messages
    {
        public static string HeaderNotFound(string key) => $"Expecting header '{key}' but was not found.";
        public static string HeaderValueValid(string key, string value) => $"Header '{key}' value '{value}' is valid.";
        internal static string HeaderValueInvalid(string key, string value) => $"Header '{key}' value '{value}' is invalid.";
        internal static string BodyValid(string content) => $"Content '{content}' is valid.";
        internal static string BodyInvalid(string content) => $"Content '{content}' is invalid.";
    }
}
