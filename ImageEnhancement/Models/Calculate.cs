using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageEnhancement.Models
{
    public static class Calculate
    {
        public static Bitmap Convolution<T>(this Bitmap sourceBitmap, T filter)
                         where T : FilterBase
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            int filterWidth = filter.Kernel.GetLength(1);
            int filterOffset = (filterWidth - 1) / 2;
            int byteOffset;

            for (int offsetY = filterOffset; offsetY <
                 sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                     sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;

                    RGB newPixelRGB = CalculateNewPixelRGBValue(filter, filterOffset, byteOffset, sourceData, pixelBuffer);

                    newPixelRGB.B = (1 / filter.Scale) * newPixelRGB.B + filter.Bias;
                    newPixelRGB.G = (1 / filter.Scale) * newPixelRGB.G + filter.Bias;
                    newPixelRGB.R = (1 / filter.Scale) * newPixelRGB.R + filter.Bias;


                    if (newPixelRGB.B > 255)
                    { newPixelRGB.B = 255; }
                    else if (newPixelRGB.B < 0)
                    { newPixelRGB.B = 0; }

                    if (newPixelRGB.G > 255)
                    { newPixelRGB.G = 255; }
                    else if (newPixelRGB.G < 0)
                    { newPixelRGB.G = 0; }

                    if (newPixelRGB.R > 255)
                    { newPixelRGB.R = 255; }
                    else if (newPixelRGB.R < 0)
                    { newPixelRGB.R = 0; }


                    resultBuffer[byteOffset] = (byte)(newPixelRGB.B);
                    resultBuffer[byteOffset + 1] = (byte)(newPixelRGB.G);
                    resultBuffer[byteOffset + 2] = (byte)(newPixelRGB.R);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = CreateResultImage(sourceBitmap, resultBuffer);

            return resultBitmap;
        }
        private static Bitmap CreateResultImage(Bitmap originalImage, byte[] resultBuffer)
        {
            Bitmap result = new Bitmap(originalImage.Width, originalImage.Height);


            BitmapData resultData = result.LockBits(new Rectangle(0, 0,
                                    result.Width, result.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            result.UnlockBits(resultData);

            return result;
        }
        private static RGB CalculateNewPixelRGBValue(FilterBase filter, int filterOffset, int byteOffset, BitmapData sourceData, 
                                                                                                          byte[] pixelBuffer)
        {
            RGB rgb = new RGB();
            int calcOffset;

            for (int filterY = -filterOffset;
                         filterY <= filterOffset; filterY++)
            {
                for (int filterX = -filterOffset;
                     filterX <= filterOffset; filterX++)
                {
                    calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);

                    if(filter is ContrastFilter)
                    {
                        rgb.B += (double)((((pixelBuffer[calcOffset] / 255.0) - 0.5) * 
                                 filter.Kernel[filterY + filterOffset, filterX + filterOffset]) + 0.5) * 255.0;
                        rgb.G += (double)((((pixelBuffer[calcOffset + 1] / 255.0) - 0.5) *
                                 filter.Kernel[filterY + filterOffset, filterX + filterOffset]) + 0.5) * 255.0;
                        rgb.R += (double)((((pixelBuffer[calcOffset + 2] / 255.0) - 0.5) *
                                 filter.Kernel[filterY + filterOffset, filterX + filterOffset]) + 0.5) * 255.0;
                    }
                    else
                    {
                        rgb.B += (double)(pixelBuffer[calcOffset]) *
                                 filter.Kernel[filterY + filterOffset, filterX + filterOffset];

                        rgb.G += (double)(pixelBuffer[calcOffset + 1]) *
                                  filter.Kernel[filterY + filterOffset, filterX + filterOffset];

                        rgb.R += (double)(pixelBuffer[calcOffset + 2]) *
                                filter.Kernel[filterY + filterOffset, filterX + filterOffset];
                    }
                }
            }

            return rgb;
        }
    }
}
