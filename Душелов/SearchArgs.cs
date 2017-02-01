// Decompiled with JetBrains decompiler
// Type: Душелов.SearchArgs
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;

namespace Душелов
{
  public class SearchArgs : EventArgs
  {
    public string Text;
    public bool FromBegin;
    public bool Exact;
    public bool WholeDoc;
    public bool FindNext;
    public bool Up;

    internal SearchArgs(string text, bool frombegin, bool exact, bool wholedoc, bool findnext, bool up)
    {
      this.Text = text;
      this.FromBegin = frombegin;
      this.Exact = exact;
      this.WholeDoc = wholedoc;
      this.FindNext = findnext;
      this.Up = up;
    }
  }
}
