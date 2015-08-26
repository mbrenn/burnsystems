//-----------------------------------------------------------------------
// <copyright file="ImageHandler.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    /// This class takes an image and performs some actions on it. 
    /// </summary>
    public class ImageHandler : IDisposable
    {
        /// <summary>
        /// Image to be handled
        /// </summary>
        private Image image;

        /// <summary>
        /// Initializes a new instance of the ImageHandler class.
        /// </summary>
        /// <param name="image">Image used by this handler</param>
        public ImageHandler(Image image)
        {
            this.image = image;
        }

        /// <summary>
        /// Initializes a new instance of the ImageHandler class.
        /// The image is read from stream.
        /// </summary>
        /// <param name="stream">Stream containing the image</param>
        public ImageHandler(Stream stream)
        {
            image = Image.FromStream(stream);
        }

        /// <summary>
        /// Initializes a new instance of the ImageHandler class.
        /// </summary>
        /// <param name="imageData">Imagedata containing the image</param>
        public ImageHandler(byte[] imageData)
        {
            using (var stream = new MemoryStream(imageData))
            {
                image = Image.FromStream(stream);
            }
        }

        /// <summary>
        /// Finalizes an instance of the ImageHandler class.
        /// </summary>
        ~ImageHandler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets JPEG-Encoder
        /// </summary>
        public static ImageCodecInfo JpegEncoder
        {
            get
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                    {
                        return codec;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the image
        /// </summary>
        public Image Image
        {
            get { return image; }
        }   

        /// <summary>
        /// Resizes the image so, that it fits to the border
        /// </summary>
        /// <param name="width">Width of border</param>
        /// <param name="height">Height of border</param>
        public void ResizeToBorder(int width, int height)
        {
            double factor = width / (double)image.Width;
            factor = System.Math.Min(factor, height / (double)image.Height);

            // Checks, if image needs to be shrinked
            if (factor > 1)
            {
                return;
            }

            int newWidth = (int)(image.Width * factor);
            int newHeight = (int)(image.Height * factor);

            Image newImage = new Bitmap(image, new Size(newWidth, newHeight));
            image.Dispose();
            image = newImage;
        }

        /// <summary>
        /// Resizes image, so it fits into the border
        /// </summary>
        /// <param name="size">Targetsize of image</param>
        public void ResizeToBorder(Size size)
        {
            ResizeToBorder(size.Width, size.Height);
        }

        /// <summary>
        /// Clones image
        /// </summary>
        /// <returns>Cloned image</returns>
        public Image CloneImage()
        {
            return (Image)image.Clone();
        }

        #region IDisposable Members

        /// <summary>
        /// Vernichtet das Objekt
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the image.
        /// </summary>
        /// <param name="disposing">true, if called by Dispose()</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }
        }

        #endregion
    }
}
