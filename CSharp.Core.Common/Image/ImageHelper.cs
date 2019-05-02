using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace CSharp.Core.Common
{
    /// <summary>
    /// ImageHelper
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 放大縮小圖示
        /// </summary>
        /// <param name="image">圖示</param>
        /// <param name="ratio">比例</param>
        /// <returns></returns>
        public static Image Scale(Image image, double ratio)
        {
            Image newImage = null;
            if (image != null && ratio > 0)
            {
                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);
                newImage = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }
            }
            return newImage;
        }

        /// <summary>
        /// 從byte陣列轉Image
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static Image FromByteArray(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            return Image.FromStream(ms);
        }
    }
}
