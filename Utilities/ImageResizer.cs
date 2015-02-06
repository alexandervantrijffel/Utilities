using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Structura.SharedComponents.Utilities
{
    public static class ImageResizer
    {
        private static string TempImageLocation
        {
            get
            {
                var path = ConfigurationManager.AppSettings["TempImageLocation"];
                Check.Require(!string.IsNullOrEmpty(path), "App setting TempImageLocation not found");
                return path;
            }
        }

        private static string ResizedImagesOutputLocation
        {
            get
            {
                var path = ConfigurationManager.AppSettings["ResizedImagesOutputLocation"];
                Check.Require(!string.IsNullOrEmpty(path), "App setting TempImageLocation not found");
                return path;
            }
        }

        public static ResizeResult ResizeImage(string inputFileName, string outputFileName, int maxWidth, int maxHeight)
        {
            return ResizeImage(inputFileName, outputFileName, maxWidth, maxHeight, TempImageLocation, ResizedImagesOutputLocation);
        }

        /// <returns></returns>
        public static ResizeResult ResizeImage(string inputFileName, string outputFileName
            , int maxWidth, int maxHeight, string inputFolder, string outputFolder)
        {
            var inputFilePath = Path.Combine(inputFolder, inputFileName);
            var outputFilePath = Path.Combine(outputFolder, outputFileName);
            var resultFileName = outputFileName;
            using (var readStream = File.OpenRead(inputFilePath))
            {
                var theImage = Image.FromStream(readStream);
                var result = new ResizeResult();

                if (theImage.Width > maxWidth || theImage.Height > maxHeight)
                {
                    // resize if width or height is more than the given values
                    ImageFormat inputFormat;
                    var resizedImage = ResizeImage(theImage, maxWidth, maxHeight, out inputFormat);
                    resizedImage.Save(outputFilePath);
                }
                else
                {
                    readStream.Seek(0, SeekOrigin.Begin);

                    // otherwise save original file
                    using (var fileStream = File.Create(outputFilePath))
                    {
                        readStream.CopyTo(fileStream);
                    }
                }
                result.FilePath = outputFilePath;
                result.FileName = resultFileName;
                return result;
            }
        }

        public static Image ResizeImage(Image image, int maxWidth, int maxHeight, out ImageFormat imageFormat)
        {
            var srcBitmap = image;
            int newWidth;
            int newHeight;

            var widthPercentage = (float)maxWidth / srcBitmap.Width;
            var heightPercentage = (float)maxHeight / srcBitmap.Height;
            var resizePercentage = Math.Min(widthPercentage, heightPercentage);
            if (resizePercentage < 1)
            {
                newWidth = (int)Math.Round(srcBitmap.Width * resizePercentage);
                newHeight = (int)Math.Round(srcBitmap.Height * resizePercentage);
            }
            else
            {
                // no need to downsize
                imageFormat = srcBitmap.RawFormat;
                return srcBitmap;
            }

            var newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.DrawImage(srcBitmap, new Rectangle(0, 0, newWidth, newHeight));
            }
            imageFormat = newImage.RawFormat;
            return newImage;
        }

        public static Image ResizeImage(Stream imageStream, int maxWidth, int maxHeight, out ImageFormat imageFormat)
        {
            return ResizeImage(Image.FromStream(imageStream), maxWidth, maxHeight, out imageFormat);
        }
    }

    public class ResizeResult
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}