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

// (c) by BurnSystems '06

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace BurnSystems.Graphics
{
    /// <summary>
    /// This class takes an image and performs some actions on it. 
    /// </summary>
    public class ImageHandler : IDisposable
    {
        /// <summary>
        /// Image to be handled
        /// </summary>
        Image m_oImage;

        /// <summary>
        /// Current image
        /// </summary>
        public Image Image
        {
            get { return m_oImage; }
        }   

        /// <summary>
        /// Creates new image handler and stores the imageit
        /// </summary>
        /// <param name="oImage">Image</param>
        public ImageHandler(Image oImage)
        {
            m_oImage = oImage;
        }

        /// <summary>
        /// Creates a new image and creates an image from stream
        /// </summary>
        /// <param name="oStream">Stream</param>
        public ImageHandler(Stream oStream)
        {
            m_oImage = Image.FromStream(oStream);
        }

        public ImageHandler(byte[] aoImageData)
        {
            using (MemoryStream oStream = new MemoryStream(aoImageData))
            {
                m_oImage = Image.FromStream(oStream);
            }
        }

        /// <summary>
        /// Resizes the image so, that it fits to the border
        /// </summary>
        /// <param name="nWidth">Width</param>
        /// <param name="nHeight">Height</param>
        public void ResizeToBorder(int nWidth, int nHeight)
        {
            double dFactor = nWidth / (double)m_oImage.Width;
            dFactor = System.Math.Min(dFactor, nHeight / (double)m_oImage.Height);

            // Checks, if image needs to be shrinked
            if (dFactor > 1)
            {
                return;
            }

            int nNewWidth = (int)(m_oImage.Width * dFactor);
            int nNewHeight = (int)(m_oImage.Height * dFactor);

            Image oNewImage = new Bitmap(m_oImage, new Size(nNewWidth, nNewHeight));
            m_oImage.Dispose();
            m_oImage = oNewImage;
        }


        /// <summary>
        /// Resizes image, so it fits into the border
        /// </summary>
        /// <param name="oSize">Targetsize of image</param>
        public void ResizeToBorder(Size oSize)
        {
            ResizeToBorder(oSize.Width, oSize.Height);
        }

        /// <summary>
        /// Clones image
        /// </summary>
        /// <returns></returns>
        public Image CloneImage()
        {
            return (Image)m_oImage.Clone();
        }

        /// <summary>
        /// Gets JPEG-Encoder
        /// </summary>
        public static ImageCodecInfo JpegEncoder
        {
            get
            {
                ImageCodecInfo[] aoCodecs = ImageCodecInfo.GetImageEncoders();

                foreach (ImageCodecInfo oCodec in aoCodecs)
                {
                    if (oCodec.MimeType == "image/jpeg")
                    {
                        return oCodec;
                    }
                }

                return null;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the image.
        /// </summary>
        void Dispose(bool bDisposing)
        {
            if (bDisposing)
            {
                if (m_oImage != null)
                {
                    m_oImage.Dispose();
                    m_oImage = null;
                }                
            }
        }

        /// <summary>
        /// Vernichtet das Objekt
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalisierer
        /// </summary>
        ~ImageHandler()
        {
            Dispose(false);
        }

        #endregion

    }
}
