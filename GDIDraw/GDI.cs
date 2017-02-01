// Decompiled with JetBrains decompiler
// Type: GDIDraw.GDI
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GDIDraw
{
  public class GDI
  {
    private IntPtr hdc;
    private Graphics grp;

    public GDI(IntPtr ptrHdc)
    {
      this.grp = (Graphics) null;
      this.hdc = ptrHdc;
    }

    public GDI(Graphics g)
    {
      this.grp = g;
      this.hdc = this.grp.GetHdc();
    }

    ~GDI()
    {
      if (this.grp == null)
        return;
      this.grp.ReleaseHdc(this.hdc);
    }

    public void DrawLine(Color color, Point p1, Point p2)
    {
      this.SetROP2(drawingMode.R2_XORPEN);
      IntPtr pen = this.CreatePEN(PenStyles.PS_SOLID, 2, GDI.RGB((int) color.R, (int) color.G, (int) color.B));
      IntPtr hgdiobj = this.SelectObject(pen);
      this.MoveTo(p1.X, p1.Y);
      this.LineTo(p2.X, p2.X);
      this.SelectObject(hgdiobj);
      this.DeleteOBJECT(pen);
    }

    public IntPtr CreatePEN(PenStyles fnPenStyle, int nWidth, int crColor)
    {
      return GDI.CreatePen(fnPenStyle, nWidth, crColor);
    }

    public bool DeleteOBJECT(IntPtr hObject)
    {
      return GDI.DeleteObject(hObject);
    }

    public IntPtr SelectObject(IntPtr hgdiobj)
    {
      return GDI.SelectObject(this.hdc, hgdiobj);
    }

    public void MoveTo(int X, int Y)
    {
      GDI.MoveToEx(this.hdc, X, Y, 0);
    }

    public void LineTo(int X, int Y)
    {
      GDI.LineTo(this.hdc, X, Y);
    }

    public int SetROP2(drawingMode fnDrawMode)
    {
      return GDI.SetROP2(this.hdc, fnDrawMode);
    }

    [DllImport("gdi32.dll")]
    public static extern int SetROP2(IntPtr hdc, drawingMode fnDrawMode);

    [DllImport("gdi32.dll")]
    public static extern bool MoveToEx(IntPtr hdc, int X, int Y, int oldp);

    [DllImport("gdi32.dll")]
    public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreatePen(PenStyles fnPenStyle, int nWidth, int crColor);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    public static int RGB(int R, int G, int B)
    {
      return R | G << 8 | B << 16;
    }
  }
}
