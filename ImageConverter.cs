using System.IO;
using System.Windows.Media.Imaging;
using System.Net;
using System.Drawing.Printing;
using System;
using System.Windows.Data;
using System.Collections.Generic;

namespace _20
{
    public class ImageConverter
    {
        public static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        public static BitmapImage ByteArrayToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public static BitmapImage StreamToImage(Stream ms)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static string CurrentIp() // ayyy lmao refactoring
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());            
            string ip = ipEntry.AddressList[1].ToString();
            return ip;
        }
    }
}
