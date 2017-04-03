using System;

namespace rori.Network.Upload
{
    public class UploadServiceInfo
    {
        public UploadServiceInfo(Uri result, bool success)
        {
            _result = result;
            _success = success;
        }

        private Uri _result;
        public Uri Result { get { return _result; } }

        private bool _success;
        public bool Success { get { return _success; } }
    }
}