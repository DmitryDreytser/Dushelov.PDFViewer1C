// Decompiled with JetBrains decompiler
// Type: Gma.UserActivityMonitor.MouseEventExtArgs
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
  public class MouseEventExtArgs : MouseEventArgs
  {
    private bool m_Handled;

    public bool Handled
    {
      get
      {
        return this.m_Handled;
      }
      set
      {
        this.m_Handled = value;
      }
    }

    public MouseEventExtArgs(MouseButtons buttons, int clicks, int x, int y, int delta)
      : base(buttons, clicks, x, y, delta)
    {
    }

    internal MouseEventExtArgs(MouseEventArgs e)
      : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
    {
    }
  }
}
