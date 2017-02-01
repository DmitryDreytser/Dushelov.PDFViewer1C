// Decompiled with JetBrains decompiler
// Type: PDFViewer.RawPrinterHelper
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;

namespace PDFViewer
{
  public class RawPrinterHelper
  {
    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [MarshalAs(UnmanagedType.LPStruct), In] RawPrinterHelper.DOCINFOA di);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int capindex);

    public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
    {
      int dwWritten = 0;
      IntPtr hPrinter = new IntPtr(0);
      RawPrinterHelper.DOCINFOA di = new RawPrinterHelper.DOCINFOA();
      bool flag = false;
      di.pDocName = "My C#.NET RAW Document";
      di.pDataType = "RAW";
      if (RawPrinterHelper.OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
      {
        if (RawPrinterHelper.StartDocPrinter(hPrinter, 1, di))
        {
          if (RawPrinterHelper.StartPagePrinter(hPrinter))
          {
            flag = RawPrinterHelper.WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
            RawPrinterHelper.EndPagePrinter(hPrinter);
          }
          RawPrinterHelper.EndDocPrinter(hPrinter);
        }
        RawPrinterHelper.ClosePrinter(hPrinter);
      }
      if (!flag)
        Marshal.GetLastWin32Error();
      return flag;
    }

    public static bool SendFileToPrinter(string szPrinterName, string szFileName)
    {
      FileStream fileStream = new FileStream(szFileName, FileMode.Open);
      BinaryReader binaryReader = new BinaryReader((Stream) fileStream);
      byte[] numArray = new byte[fileStream.Length];
      IntPtr num1 = new IntPtr(0);
      int int32 = Convert.ToInt32(fileStream.Length);
      byte[] source = binaryReader.ReadBytes(int32);
      IntPtr num2 = Marshal.AllocCoTaskMem(int32);
      Marshal.Copy(source, 0, num2, int32);
      bool printer = RawPrinterHelper.SendBytesToPrinter(szPrinterName, num2, int32);
      Marshal.FreeCoTaskMem(num2);
      return printer;
    }

    public static bool SendStringToPrinter(string szPrinterName, string szString)
    {
      int length = szString.Length;
      IntPtr coTaskMemAnsi = Marshal.StringToCoTaskMemAnsi(szString);
      RawPrinterHelper.SendBytesToPrinter(szPrinterName, coTaskMemAnsi, length);
      Marshal.FreeCoTaskMem(coTaskMemAnsi);
      return true;
    }

    public static int DotsPerMM(PrinterSettings pr)
    {
      if (pr != null)
        return (int) ((double) pr.DefaultPageSettings.PrinterResolution.X / 25.375);
      return 8;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DOCINFOA
    {
      [MarshalAs(UnmanagedType.LPStr)]
      public string pDocName;
      [MarshalAs(UnmanagedType.LPStr)]
      public string pOutputFile;
      [MarshalAs(UnmanagedType.LPStr)]
      public string pDataType;
    }
  }
}
