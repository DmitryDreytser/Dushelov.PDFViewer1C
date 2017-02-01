// Decompiled with JetBrains decompiler
// Type: PDFViewer.ShadowDrawing
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System.Drawing;
using System.Drawing.Drawing2D;
using Душелов.Properties;

namespace PDFViewer
{
  public class ShadowDrawing
  {
    private static int shadowSize = 5;
    private static int shadowMargin = 2;
    private static Image shadowDownRight = (Image) new Bitmap((Image) Resources.tshadowdownright);
    private static Image shadowDownLeft = (Image) new Bitmap((Image) Resources.tshadowdownleft);
    private static Image shadowDown = (Image) new Bitmap((Image) Resources.tshadowdown);
    private static Image shadowRight = (Image) new Bitmap((Image) Resources.tshadowright);
    private static Image shadowTopRight = (Image) new Bitmap((Image) Resources.tshadowtopright);

    public static void DrawShadow(Graphics g, Rectangle r)
    {
      r.Offset(ShadowDrawing.shadowSize, ShadowDrawing.shadowSize);
      TextureBrush textureBrush1 = new TextureBrush(ShadowDrawing.shadowRight, WrapMode.Tile);
      TextureBrush textureBrush2 = new TextureBrush(ShadowDrawing.shadowDown, WrapMode.Tile);
      textureBrush2.TranslateTransform(0.0f, (float) (r.Height + r.Y - ShadowDrawing.shadowSize));
      textureBrush1.TranslateTransform((float) (r.Width + r.X - ShadowDrawing.shadowSize), 0.0f);
      Rectangle rect1 = new Rectangle(r.X + ShadowDrawing.shadowSize + ShadowDrawing.shadowMargin, r.Y + r.Height - ShadowDrawing.shadowSize, r.Width - (2 * ShadowDrawing.shadowSize + ShadowDrawing.shadowMargin), ShadowDrawing.shadowSize);
      Rectangle rect2 = new Rectangle(r.Width - ShadowDrawing.shadowSize + r.X, ShadowDrawing.shadowSize + ShadowDrawing.shadowMargin + r.Y, ShadowDrawing.shadowSize, r.Height - (ShadowDrawing.shadowSize * 2 + ShadowDrawing.shadowMargin));
      g.FillRectangle((Brush) textureBrush2, rect1);
      g.FillRectangle((Brush) textureBrush1, rect2);
      g.DrawImage(ShadowDrawing.shadowTopRight, new Rectangle(r.Width - ShadowDrawing.shadowSize + r.X, ShadowDrawing.shadowMargin + r.Y, ShadowDrawing.shadowSize, ShadowDrawing.shadowSize));
      g.DrawImage(ShadowDrawing.shadowDownRight, new Rectangle(r.Width - ShadowDrawing.shadowSize + r.X, r.Height - ShadowDrawing.shadowSize + r.Y, ShadowDrawing.shadowSize, ShadowDrawing.shadowSize));
      g.DrawImage(ShadowDrawing.shadowDownLeft, new Rectangle(ShadowDrawing.shadowMargin + r.X, r.Height - ShadowDrawing.shadowSize + r.Y, ShadowDrawing.shadowSize, ShadowDrawing.shadowSize));
      textureBrush2.Dispose();
      textureBrush1.Dispose();
    }

    public static void DrawBorderLeft(Graphics g, Point p)
    {
    }

    public static void DrawBorderBottom(Graphics g, Point p)
    {
    }

    public static void DrawBorderDownRight(Graphics g, Point p)
    {
    }

    public static void DrawBorderRight(Graphics g, Point p)
    {
    }

    public static void DrawBorderTopRight(Graphics g, Point p)
    {
    }
  }
}
