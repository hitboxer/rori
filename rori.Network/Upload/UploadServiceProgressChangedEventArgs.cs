using System.ComponentModel;

namespace rori.Network.Upload
{
    public class UploadServiceProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public UploadServiceProgressChangedEventArgs(int progressPercentage, object userState, int transferredBytes, long? totalBytes) : base(progressPercentage, userState)
        {
            TransferredBytes = transferredBytes;
            TotalBytes = totalBytes;
        }

        public int TransferredBytes { get; private set; }

        public long? TotalBytes { get; private set; }
    }
}