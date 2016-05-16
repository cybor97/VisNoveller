using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VNCore.Extensions
{
    public static class BitmapConverter
    {
        public static byte[] ToByteArray(this Icon icon)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                icon.ToBitmap().Save(ms, ImageFormat.Png);
                return ms.GetBuffer();
            }
        }
        public static Icon ToIcon(this byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes)) return Icon.FromHandle(new Bitmap(ms).GetHicon());
        }
        public static byte[] ToByteArray(this Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            return ms.GetBuffer();
        }
        public static Bitmap ToBitmap(this byte[] ByteArray)
        {
            return (Bitmap)Image.FromStream(new MemoryStream(ByteArray)); ;
        }
    }
}
