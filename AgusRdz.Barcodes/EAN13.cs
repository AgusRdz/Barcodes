using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace AgusRdz.Barcodes
{
    public class EAN13 : IDisposable
    {
        #region Private Members
        private int _checksum = 0;
        private int _first = 0;
        private int _width = 0;
        private int _height = 0;
        private bool _isTableA = false;
        private bool _disposed = false;
        private string _result = "";      
        private Font _fontNormal = new Font("Arial", 8, FontStyle.Regular);
        private Font _fontBarcode = new Font("Code EAN13", 48, FontStyle.Regular);
        private Graphics _graphics = Graphics.FromImage(new Bitmap(1, 1));
        private Bitmap _bmp;
        #endregion

        /// <summary>
        /// Create an image with the EAN13 bar-code.
        /// </summary>
        /// <param name="barcode">Numeric string to parse.</param>
        /// <param name="path">Path and name of iamge to save the image(must include format extension)</param>
        /// <param name="format">Format to save the image</param>
        public void CreateBarcode(string barcode, string path, ImageFormat format)
        {
            barcode = ConvertToEAN13(barcode);
            _width = (int)_graphics.MeasureString(barcode, _fontBarcode).Width;
            _height = (int)_graphics.MeasureString(barcode, _fontBarcode).Height;
            _bmp = new Bitmap(_width + 10, _height + 10, PixelFormat.Format32bppArgb);
            _graphics = Graphics.FromImage(_bmp);
            _graphics.Clear(Color.White);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            _graphics.DrawString(barcode, _fontBarcode, Brushes.Black, new PointF(0, 10));
            _graphics.Save();
            _graphics.Flush();
            _bmp.Save(path, format);
        }

        /// <summary>
        /// Create an image with the EAN13 bar-code.
        /// </summary>
        /// <param name="barcode">Numeric string to parse.</param>
        /// <param name="product">Name of product</param>
        /// <param name="path">Path and name of iamge to save the image(must include format extension)</param>
        /// <param name="format">Format to save the image</param>
        public void CreateBarcode(string barcode, string product, string path, ImageFormat format)
        {
            barcode = ConvertToEAN13(barcode);
            if (_graphics.MeasureString(product, _fontNormal).Width > _graphics.MeasureString(barcode, _fontBarcode).Width)
                _width = (int)_graphics.MeasureString(product, _fontNormal).Width;
            else
                _width = (int)_graphics.MeasureString(barcode, _fontBarcode).Width;

            _height = (int)_graphics.MeasureString(barcode, _fontBarcode).Height;
            _bmp = new Bitmap(_width + 20, _height + 20, PixelFormat.Format32bppArgb);
            _graphics = Graphics.FromImage(_bmp);
            _graphics.Clear(Color.White);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            _graphics.DrawString(product, _fontNormal, Brushes.Black, new PointF(14, 2));
            _graphics.DrawString(barcode, _fontBarcode, Brushes.Black, new PointF(0, 10));
            _graphics.Save();
            _graphics.Flush();
            _bmp.Save(path, format);
        }

        /// <summary>
        /// Create an image with the EAN13 bar-code.
        /// </summary>
        /// <param name="barcode">Numeric string to parse.</param>
        /// <param name="product">Name of product</param>
        /// <param name="details">Specific details of product</param>
        /// <param name="path">Path and name of iamge to save the image(must include format extension)</param>
        /// <param name="format">Format to save the image</param>
        public void CreateBarcode(string barcode, string product, string details, string path, ImageFormat format)
        {
            barcode = ConvertToEAN13(barcode);
            if (_graphics.MeasureString(product, _fontNormal).Width > _graphics.MeasureString(barcode, _fontBarcode).Width)
                _width = (int)_graphics.MeasureString(product, _fontNormal).Width + 8;
            else
                _width = (int)_graphics.MeasureString(barcode, _fontBarcode).Width + 8;

            _height = (int)_graphics.MeasureString(barcode, _fontBarcode).Height;
            _bmp = new Bitmap(_width + 20, _height + 20, PixelFormat.Format32bppArgb);
            _graphics = Graphics.FromImage(_bmp);
            _graphics.Clear(Color.White);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            _graphics.DrawString(product, _fontNormal, Brushes.Black, new PointF(14, 2));
            _graphics.DrawString(barcode, _fontBarcode, Brushes.Black, new PointF(0, 10));
            _graphics.DrawString(details, _fontNormal, Brushes.Black, new PointF(_width - 25, 36));
            _graphics.Save();
            _graphics.Flush();
            _bmp.Save(path, format);
        }

        /// <summary>
        /// Convert decimal code to EAN13 standard code.
        /// </summary>
        /// <param name="barCode">Decimal code to convert.</param>
        /// <returns>EAN13 standard code.</returns>
        private string ConvertToEAN13(string barCode)
        {
            int i;

            for (i = 1; i <= barCode.Length; i++)
            {
                int L1 = Convert.ToChar(barCode.Substring(i - 1, 1));
                if (L1 < 48 || L1 > 57)
                {
                    i = 0;
                    break;
                }
            }

            if (i == 13)
            {
                for (i = 12; i >= 1; i += -2)
                    _checksum += Convert.ToInt32(barCode.Substring(i - 1, 1));

                _checksum *= 3;

                for (i = 11; i >= 1; i += -2)
                    _checksum += Convert.ToInt32(barCode.Substring(i - 1, 1));

                barCode += (10 - _checksum % 10) % 10;
                _result = barCode.Substring(0, 1) + Convert.ToChar(65 + Convert.ToInt32(barCode.Substring(1, 1)));
                _first = Convert.ToInt32(barCode.Substring(0, 1));

                for (i = 3; i <= 7; i++)
                {
                    _isTableA = false;
                    switch (i)
                    {
                        case 3:
                            switch (_first)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    _isTableA = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (_first)
                            {
                                case 0:
                                case 4:
                                case 7:
                                case 8:
                                    _isTableA = true;
                                    break;
                            }
                            break;
                        case 5:
                            switch (_first)
                            {
                                case 0:
                                case 1:
                                case 4:
                                case 5:
                                case 9:
                                    _isTableA = true;
                                    break;
                            }
                            break;
                        case 6:
                            switch (_first)
                            {
                                case 0:
                                case 2:
                                case 5:
                                case 6:
                                case 7:
                                    _isTableA = true;
                                    break;
                            }
                            break;
                        case 7:
                            switch (_first)
                            {
                                case 0:
                                case 3:
                                case 6:
                                case 8:
                                case 9:
                                    _isTableA = true;
                                    break;
                            }
                            break;
                    }

                    if (_isTableA)
                        _result += Convert.ToChar(65 + Convert.ToInt32(barCode.Substring(i - 1, 1)));
                    else
                        _result += Convert.ToChar(75 + Convert.ToInt32(barCode.Substring(i - 1, 1)));
                }

                _result += "*";
                for (i = 8; i <= 13; i++)
                    _result += Convert.ToChar(97 + Convert.ToInt32(barCode.Substring(i - 1, 1)));

                _result += "+";
            }

            return _result;
        }

        /// <summary>
        /// Performs all object cleanup
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if(disposing)
            {
                _bmp.Dispose();
                _fontBarcode.Dispose();
                _fontNormal.Dispose();
            }

            _disposed = true;
        }

        ~EAN13()
        {
            Dispose(false);
        }
    }
}
