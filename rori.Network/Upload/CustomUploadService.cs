using System;
using System.Collections.Generic;

namespace rori.Network.Upload
{
    public class CustomUploadService
    {
        public CustomUploadService(string name, string fieldName, Uri requestUri, List<KeyValuePair<string, string>> arguments, List<KeyValuePair<string, string>> headers, string pattern)
        {
            _name = name;
            _fieldName = fieldName;
            _requestUri = requestUri;
            _arguments = arguments;
            _headers = headers;
            _pattern = pattern;
        }

        private string _name;
        public string Name { get { return _name; } }

        private string _fieldName;
        public string FieldName { get { return _fieldName; } }

        private Uri _requestUri;
        public Uri RequestUri { get { return _requestUri; } }

        private List<KeyValuePair<string, string>> _arguments;
        public List<KeyValuePair<string, string>> Arguments { get { return _arguments; } }

        private List<KeyValuePair<string, string>> _headers;
        public List<KeyValuePair<string, string>> Headers { get { return _headers; } }

        private string _pattern;
        public string Pattern { get { return _pattern; } }
    }
}