using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace rori.Network.Upload
{
    public class ProgressStream : MemoryStream
    {
        public ProgressStream(byte[] buffer, UploadService.UploadServiceProgressChangedEventHandler handler) : base(buffer)
        {
            UploadServiceProgressChanged = handler;
        }

        private long BytesReceived { get; set; }

        public event UploadService.UploadServiceProgressChangedEventHandler UploadServiceProgressChanged;

        protected virtual void OnUploadServiceProgressChanged(UploadServiceProgressChangedEventArgs e)
        {
            var handler = UploadServiceProgressChanged;
            if (handler != null) handler(this, e);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return ReportBytesReceived(base.Read(buffer, offset, count));
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return ReportBytesReceived(await base.ReadAsync(buffer, offset, count, cancellationToken));
        }

        public override int ReadByte()
        {
            return ReportBytesReceived(base.ReadByte());
        }

        private int ReportBytesReceived(int bytesReceived)
        {
            if (bytesReceived > 0)
            {
                BytesReceived += bytesReceived;

                var percentage = 0;

                if (Length != 0) percentage = (int)((100L * BytesReceived) / Length);

                OnUploadServiceProgressChanged(new UploadServiceProgressChangedEventArgs(percentage, null, (int)BytesReceived, Length));
            }

            return bytesReceived;
        }
    }
}