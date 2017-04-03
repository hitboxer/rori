using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace rori.Helpers.Extension
{
    public static class ExtensionMethods
    {
        public static byte[] ToByteArray(this Image image)
        {
            using (var ms = new MemoryStream())
            {
                var e = new EncoderParameters(1);
                e.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                image.Save(ms, GetEncoder(ImageFormat.Png), e);
                ms.Flush();
                return ms.ToArray();
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}