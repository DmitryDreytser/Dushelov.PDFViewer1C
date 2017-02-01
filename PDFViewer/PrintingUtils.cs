// Decompiled with JetBrains decompiler
// Type: PDFViewer.PrintingUtils
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PDFViewer
{
  public class PrintingUtils
  {
    public static Color ColorToGrey(Color c)
    {
      int num = (int) ((double) c.R * 0.3 + (double) c.G * 0.59 + (double) c.B * 0.11);
      return Color.FromArgb(num, num, num);
    }

    public static Bitmap GrayScale(Bitmap Bitmap)
    {
      Bitmap bitmap = (Bitmap) Bitmap.Clone();
      for (int x = 0; x < bitmap.Width; ++x)
      {
        for (int y = 0; y < bitmap.Height; ++y)
        {
          Color grey = PrintingUtils.ColorToGrey(bitmap.GetPixel(x, y));
          Bitmap.SetPixel(x, y, grey);
        }
      }
      return Bitmap;
    }

    private static void SetIndexedPixel(int x, int y, BitmapData bmd, bool pixel)
    {
      int ofs = y * bmd.Stride + (x >> 3);
      byte num1 = Marshal.ReadByte(bmd.Scan0, ofs);
      byte num2 = (byte) (128 >> (x & 7));
      byte val = !pixel ? (byte) ((uint) num1 & (uint) (byte) ((uint) num2 ^ (uint) byte.MaxValue)) : (byte) ((uint) num1 | (uint) num2);
      Marshal.WriteByte(bmd.Scan0, ofs, val);
    }

    private static int BitsPerPixel(Bitmap img)
    {
      switch (img.PixelFormat)
      {
        case PixelFormat.Format16bppGrayScale:
        case PixelFormat.Format16bppArgb1555:
        case PixelFormat.Format16bppRgb555:
        case PixelFormat.Format16bppRgb565:
          return 16;
        case PixelFormat.Format48bppRgb:
          return 48;
        case PixelFormat.Format32bppArgb:
        case PixelFormat.Format32bppRgb:
          return 32;
        case PixelFormat.Format8bppIndexed:
          return 8;
        case PixelFormat.Format1bppIndexed:
          return 1;
        case PixelFormat.Format4bppIndexed:
          return 4;
        case PixelFormat.Format24bppRgb:
          return 24;
        default:
          throw new Exception("Error Unsoported Pixel Format");
      }
    }

    public static Bitmap Monocrome(Bitmap imgSource, bool inverted)
    {
      Rectangle rect = new Rectangle(0, 0, imgSource.Width, imgSource.Height);
      Bitmap bitmap = new Bitmap(imgSource.Width, imgSource.Height, PixelFormat.Format1bppIndexed);
      BitmapData bitmapdata = imgSource.LockBits(rect, ImageLockMode.ReadOnly, imgSource.PixelFormat);
      BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
      for (int y = 0; y < bitmap.Height; ++y)
      {
        for (int x = 0; x < bitmap.Width; ++x)
        {
          int ofs = y * bitmapdata.Stride + x * PrintingUtils.BitsPerPixel(imgSource) / 8;
          Color color = Color.FromArgb((int) Marshal.ReadByte(bitmapdata.Scan0, ofs + 2), (int) Marshal.ReadByte(bitmapdata.Scan0, ofs + 1), (int) Marshal.ReadByte(bitmapdata.Scan0, ofs));
          if (!inverted && (double) color.GetBrightness() > 0.550000011920929 || inverted && inverted && (double) color.GetBrightness() < 0.550000011920929)
            PrintingUtils.SetIndexedPixel(x, y, bitmapData, true);
        }
      }
      bitmap.UnlockBits(bitmapData);
      imgSource.UnlockBits(bitmapdata);
      return bitmap;
    }

    public static Bitmap ResizeBitmap(Bitmap bitmap, SizeF newSize)
    {
      Bitmap bitmap1 = new Bitmap((int) newSize.Width, (int) newSize.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap1))
        graphics.DrawImage((Image) bitmap, 0.0f, 0.0f, newSize.Width, newSize.Height);
      return bitmap1;
    }

    public static Bitmap RawToImage(string hexRaw, PixelFormat pixelFormat, Size size)
    {
      Bitmap bitmap = (Bitmap) null;
      BitmapData bitmapdata = (BitmapData) null;
      try
      {
        int discarded = 0;
        byte[] bytes = HexEncoding.GetBytes(hexRaw, out discarded);
        bitmap = new Bitmap(size.Width, size.Height, pixelFormat);
        bitmapdata = bitmap.LockBits(new Rectangle(Point.Empty, size), ImageLockMode.WriteOnly, pixelFormat);
        Marshal.Copy(bytes, 0, bitmapdata.Scan0, bytes.Length);
        return bitmap;
      }
      finally
      {
        if (bitmapdata != null)
          bitmap.UnlockBits(bitmapdata);
      }
    }

    public static string ImageToRaw(Bitmap bmBitmap)
    {
      BitmapData bitmapdata = (BitmapData) null;
      try
      {
        if (bmBitmap.PixelFormat != PixelFormat.Format1bppIndexed)
          bmBitmap = PrintingUtils.Monocrome(bmBitmap, false);
        bitmapdata = bmBitmap.LockBits(new Rectangle(0, 0, bmBitmap.Width, bmBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);
        int length = bitmapdata.Height * bitmapdata.Stride;
        byte[] numArray = new byte[length];
        Marshal.Copy(bitmapdata.Scan0, numArray, 0, length);
        return HexEncoding.ToString(numArray);
      }
      finally
      {
        if (bitmapdata != null)
          bmBitmap.UnlockBits(bitmapdata);
      }
    }
  }
}
