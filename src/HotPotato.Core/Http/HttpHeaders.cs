using System;
using System.Collections;
using System.Collections.Generic;

namespace HotPotato.Core.Http
{
    public class HttpHeaders : IEnumerable<KeyValuePair<string, List<string>>>
    {
        private readonly Dictionary<string, List<string>> data;

        public HttpHeaders()
        {
            StringComparer ignoreCase = StringComparer.OrdinalIgnoreCase;
            data = new Dictionary<string, List<string>>(ignoreCase);
        }

        public void Add(string key, string value)
        {
            if (!this.data.ContainsKey(key)) this.data[key] = new List<string>();
            
            this.data[key].Add(value);
        }
        
        public void Add(string key, IEnumerable<string> value)
        {
            foreach (string item in value)
            {
                Add(key, item);
            }
        }

        public bool ContainsKey(string key)
        {
            return this.data.ContainsKey(key);
        }

        #region Indexer

        public List<string> this[string key]
        {
            get { return this.data[key]; }
        }

        #endregion

        #region IEnumerable Implementation
        public IEnumerator<KeyValuePair<string, List<string>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, List<string>>>)data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, List<string>>>)data).GetEnumerator();
        } 
        #endregion
    }
}
