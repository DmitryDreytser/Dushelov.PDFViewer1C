// Decompiled with JetBrains decompiler
// Type: PDFViewer.PageViewer
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using GDIDraw;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PDFViewer
{
  public class PageViewer : UserControl
  {
    private Rectangle EmptyRectangle = new Rectangle(-1, -1, 0, 0);
    private PageViewer.CursorStatus _cursorStatus = PageViewer.CursorStatus.Move;
    private Point _pointStart = Point.Empty;
    private Point _pointCurrent = Point.Empty;
    private Point _bMouseCapturedStart = Point.Empty;
    private bool InvalidateScrollBarChanged = true;
    private PageViewer.DoubleBufferMethod _PaintMethod = PageViewer.DoubleBufferMethod.BuiltInOptimizedDoubleBuffer;
    private bool bHasMorePagesT = true;
    private bool bHasMorePagesD = true;
    private const Graphics NO_BUFFER_GRAPHICS = null;
    private const Bitmap NO_BACK_BUFFER = null;
    private const BufferedGraphics NO_MANAGED_BACK_BUFFER = null;
    private bool _drawShadow;
    private bool _drawRect;
    private Color _bgColor;
    private Color _rectColor;
    private bool _bMouseCaptured;
    private int _pointX;
    private int _pointY;
    private Size _viewSize;
    private Rectangle _viewBounds;
    private Size _clientSize;
    private Rectangle _clientBounds;
    private Point _clientLocation;
    private Size _pageSize;
    private Rectangle _pageBounds;
    private Point _pageLocation;
    private Rectangle _currentView;
    private Size _scrollbarSize;
    private Bitmap BackBuffer;
    private Graphics BufferGraphics;
    private BufferedGraphicsContext GraphicManager;
    private BufferedGraphics ManagedBackBuffer;
    private HScrollBar hsb;
    private VScrollBar vsb;
    private int _deltasCount;

    public bool DrawShadow
    {
      get
      {
        return this._drawShadow;
      }
      set
      {
        this._drawShadow = value;
        this.Invalidate();
      }
    }

    public bool DrawBorder
    {
      get
      {
        return this._drawRect;
      }
      set
      {
        this._drawRect = value;
        this.Invalidate();
      }
    }

    public Color PageColor
    {
      get
      {
        return this._bgColor;
      }
      set
      {
        this._bgColor = value;
        this.Invalidate();
      }
    }

    public Color BorderColor
    {
      get
      {
        return this._rectColor;
      }
      set
      {
        this._rectColor = value;
        this.Invalidate();
      }
    }

    public PageViewer.DoubleBufferMethod PaintMethod
    {
      get
      {
        return this._PaintMethod;
      }
      set
      {
        this._PaintMethod = value;
        this.RemovePaintMethods();
        switch (value)
        {
          case PageViewer.DoubleBufferMethod.BuiltInDoubleBuffer:
            this.SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
            break;
          case PageViewer.DoubleBufferMethod.BuiltInOptimizedDoubleBuffer:
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            break;
          case PageViewer.DoubleBufferMethod.ManualDoubleBuffer11:
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.BackBuffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            this.BufferGraphics = Graphics.FromImage((Image) this.BackBuffer);
            break;
          case PageViewer.DoubleBufferMethod.ManualDoubleBuffer20:
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.GraphicManager = BufferedGraphicsManager.Current;
            this.GraphicManager.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            this.ManagedBackBuffer = this.GraphicManager.Allocate(this.CreateGraphics(), this.ClientRectangle);
            break;
        }
      }
    }

    public virtual Rectangle CurrentView
    {
      get
      {
        int num1 = this.hsb.Value;
        int num2 = this.vsb.Value;
        int width1 = this.ClientSize.Width;
        int height1 = this.ClientSize.Height;
        int x = num1 - (num1 <= this.Margin.Left ? num1 : this.Margin.Left);
        int y = num2 - (num2 <= this.Margin.Top ? num2 : this.Margin.Top);
        int num3 = width1 + (this.PageSize.Width <= this.ViewSize.Width ? this.ScrollBarSize.Width : 0);
        int num4 = height1 + (this.PageSize.Height <= this.ViewSize.Height ? this.ScrollBarSize.Height : 0);
        int width2 = num3 + (x >= this.Margin.Left ? this.Margin.Size.Width : 0);
        int height2 = num4 + (y >= this.Margin.Top ? this.Margin.Size.Height : 0);
        if (x + width2 >= this.PageSize.Width)
          width2 = this.PageSize.Width - x;
        if (y + width2 >= this.PageSize.Height)
          height2 = this.PageSize.Height - y;
        this._currentView = new Rectangle(x, y, width2, height2);
        return this._currentView;
      }
    }

    public virtual Rectangle PageBounds
    {
      get
      {
        return this._pageBounds;
      }
    }

    public virtual Point PageLocation
    {
      get
      {
        return this._pageLocation;
      }
    }

    public virtual Size PageSize
    {
      get
      {
        return this._pageSize;
      }
      set
      {
        this._pageSize = value;
        this.Resized();
        this.Invalidate();
      }
    }

    public virtual Rectangle ClientBounds
    {
      get
      {
        return this._clientBounds;
      }
    }

    public virtual Point ClientLocation
    {
      get
      {
        return this._clientLocation;
      }
    }

    public new virtual Size ClientSize
    {
      get
      {
        return this._clientSize;
      }
    }

    public virtual Rectangle ViewBounds
    {
      get
      {
        return this._viewBounds;
      }
    }

    public virtual Point ViewLocation
    {
      get
      {
        return Point.Empty;
      }
    }

    public virtual Size ViewSize
    {
      get
      {
        return this._viewSize;
      }
    }

    public virtual Size ScrollBarSize
    {
      get
      {
        return this._scrollbarSize;
      }
    }

    public virtual Point ScrollPosition
    {
      get
      {
        Point point = new Point(this.hsb.Value, this.vsb.Value);
        point.Offset(-this.Margin.Left, -this.Margin.Top);
        return point;
      }
      set
      {
        Point point = value;
        point.X = Math.Max(0, point.X);
        point.Y = Math.Max(0, point.Y);
        this.hsb.Value = Math.Min(point.X, this.hsb.Maximum);
        this.vsb.Value = Math.Min(point.Y, this.vsb.Maximum);
        this.Resized();
        this.Invalidate();
      }
    }

    private bool IsActive
    {
      get
      {
        return true;
      }
    }

    public event PageViewer.MovePageHandler NextPage;

    public event PageViewer.MovePageHandler PreviousPage;

    public event PageViewer.PaintControlHandler PaintControl;

    public PageViewer()
    {
      this.InitializeComponent();
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
      Application.ApplicationExit += new EventHandler(this.MemoryCleanup);
      this._scrollbarSize = new Size(this.vsb.Width, this.hsb.Height);
      this._clientLocation = new Point(this.Margin.Left, this.Margin.Top);
      this._bgColor = this.BackColor;
      this._rectColor = Color.Black;
    }

    public Point PointUserToPage(Point p)
    {
      int dy = this.PageBounds.Y < 0 ? -2 * (this.Margin.Top - this.PageBounds.Y) : -(2 * this.Margin.Top - this.PageBounds.Y);
      int dx = this.PageBounds.X < 0 ? -2 * (this.Margin.Left - this.PageBounds.X) : -(2 * this.Margin.Left - this.PageBounds.X);
      if (this.PageBounds.Height <= this.ViewSize.Height)
        dy = this.PageBounds.Y - this.Margin.Top;
      if (this.PageBounds.Width <= this.ViewSize.Width)
        dx = this.PageBounds.X;
      Point point = p;
      point.Offset(dx, dy);
      return point;
    }

    private void InitializeComponent()
    {
      this.hsb = new HScrollBar();
      this.vsb = new VScrollBar();
      this.SuspendLayout();
      this.hsb.Dock = DockStyle.Bottom;
      this.hsb.Location = new Point(0, 402);
      this.hsb.Name = "hsb";
      this.hsb.Size = new Size(361, 17);
      this.hsb.TabIndex = 0;
      this.hsb.Scroll += new ScrollEventHandler(this.hsb_Scroll);
      this.hsb.Resize += new EventHandler(this.vsb_Resize);
      this.vsb.Dock = DockStyle.Right;
      this.vsb.Location = new Point(344, 0);
      this.vsb.Name = "vsb";
      this.vsb.Size = new Size(17, 402);
      this.vsb.TabIndex = 1;
      this.vsb.Scroll += new ScrollEventHandler(this.vScrollBar1_Scroll);
      this.vsb.Resize += new EventHandler(this.vsb_Resize);
      this.BackColor = SystemColors.AppWorkspace;
      this.Controls.Add((Control) this.vsb);
      this.Controls.Add((Control) this.hsb);
      this.Margin = new Padding(10);
      this.Name = "DoubleBufferControl";
      this.Size = new Size(361, 419);
      this.MarginChanged += new EventHandler(this.DoubleBufferControl_MarginChanged);
      this.Resize += new EventHandler(this.DoubleBufferControl_Resize);
      this.ResumeLayout(false);
    }

    private void MemoryCleanup(object sender, EventArgs e)
    {
      if (this.BufferGraphics != null)
        this.BufferGraphics.Dispose();
      if (this.BackBuffer != null)
        this.BackBuffer.Dispose();
      if (this.ManagedBackBuffer == null)
        return;
      this.ManagedBackBuffer.Dispose();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (this.DesignMode)
      {
        base.OnPaint(e);
      }
      else
      {
        switch (this._PaintMethod)
        {
          case PageViewer.DoubleBufferMethod.NoDoubleBuffer:
            base.OnPaint(e);
            this.Render(e.Graphics);
            break;
          case PageViewer.DoubleBufferMethod.BuiltInDoubleBuffer:
            this.Render(e.Graphics);
            break;
          case PageViewer.DoubleBufferMethod.BuiltInOptimizedDoubleBuffer:
            this.Render(e.Graphics);
            break;
          case PageViewer.DoubleBufferMethod.ManualDoubleBuffer20:
            this.PaintDoubleBuffer20(e.Graphics);
            break;
        }
      }
    }

    private void RemovePaintMethods()
    {
      this.DoubleBuffered = false;
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
      if (this.BufferGraphics != null)
      {
        this.BufferGraphics.Dispose();
        this.BufferGraphics = (Graphics) null;
      }
      if (this.BackBuffer != null)
      {
        this.BackBuffer.Dispose();
        this.BackBuffer = (Bitmap) null;
      }
      if (this.ManagedBackBuffer == null)
        return;
      this.ManagedBackBuffer.Dispose();
    }

    private void DoubleBufferControl_Resize(object sender, EventArgs e)
    {
      this.Resized();
      switch (this._PaintMethod)
      {
        case PageViewer.DoubleBufferMethod.ManualDoubleBuffer11:
          this.BackBuffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
          this.BufferGraphics = Graphics.FromImage((Image) this.BackBuffer);
          break;
        case PageViewer.DoubleBufferMethod.ManualDoubleBuffer20:
          this.GraphicManager.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
          if (this.ManagedBackBuffer != null)
            this.ManagedBackBuffer.Dispose();
          this.ManagedBackBuffer = this.GraphicManager.Allocate(this.CreateGraphics(), this.ClientRectangle);
          break;
      }
      this.Refresh();
    }

    private void PaintDoubleBuffer20(Graphics ControlGraphics)
    {
      try
      {
        this.Render(this.ManagedBackBuffer.Graphics);
        this.ManagedBackBuffer.Render(ControlGraphics);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void Render(Graphics TempGraphics)
    {
      Rectangle pageBounds = this.PageBounds;
      TempGraphics.Clear(this.BackColor);
      if (!this.PageBounds.Size.Equals((object) Size.Empty))
      {
        if (pageBounds.Y < this.Margin.Top)
          pageBounds.Height = pageBounds.Height + pageBounds.Y;
        Brush brush = (Brush) new SolidBrush(this._bgColor);
        TempGraphics.FillRectangle(brush, pageBounds);
      }
      if (this.PaintControl != null)
        this.PaintControl((object) this, TempGraphics);
      if (this.PageBounds.Size.Equals((object) Size.Empty))
        return;
      if (this._drawRect)
      {
        Pen pen = new Pen(this._rectColor);
        TempGraphics.DrawRectangle(pen, pageBounds);
      }
      if (!this.DrawShadow)
        return;
      ShadowDrawing.DrawShadow(TempGraphics, pageBounds);
    }

    private void ZoomChanged()
    {
      this.Resized();
      this.Invalidate();
    }

    private void Resized()
    {
      bool flag1 = this.vsb.Value == this.vsb.Maximum && this.vsb.Value > 0;
      bool flag2 = this.hsb.Value == this.hsb.Maximum && this.hsb.Value > 0;
      int width = this.Width;
      int height = this.Height;
      if (this.PageSize.Width > this.Width - this.Margin.Size.Width)
        height -= this.ScrollBarSize.Height;
      if (this.PageSize.Height > this.Height - this.Margin.Size.Height)
        width -= this.ScrollBarSize.Width;
      this._viewSize = new Size(width, height);
      this._viewBounds = new Rectangle(Point.Empty, this._viewSize);
      this._clientSize = new Size(width - this.Margin.Size.Width, height - this.Margin.Size.Height);
      this._clientBounds = new Rectangle(this.ClientLocation, this._clientSize);
      this.InvalidateScrollBarChanged = false;
      if (this.PageSize.Height - this.ClientSize.Height > 0)
      {
        this.vsb.Maximum = (this.PageSize.Height - this.ClientSize.Height) / 2 + this.Margin.Size.Height;
        this.vsb.Visible = true;
        if (flag1)
          this.vsb.Value = this.vsb.Maximum;
      }
      else
      {
        this.vsb.Maximum = 0;
        this.hsb.Value = 0;
        this.hsb.Visible = false;
      }
      if (this.PageSize.Width - this.ClientSize.Width > 0)
      {
        this.hsb.Maximum = (this.PageSize.Width - this.ClientSize.Width) / 2 + this.Margin.Size.Width;
        this.hsb.Visible = true;
        if (flag2)
          this.hsb.Value = this.hsb.Maximum;
      }
      else
      {
        this.hsb.Maximum = 0;
        this.hsb.Value = 0;
        this.hsb.Visible = false;
      }
      this.InvalidateScrollBarChanged = true;
      this.RecalcPageLocation();
    }

    private void RecalcPageLocation()
    {
      this._pageLocation = this.PageSize.Width >= this.ClientBounds.Width || this.PageSize.Height <= this.ClientBounds.Height ? (this.PageSize.Width <= this.ClientBounds.Width || this.PageSize.Height >= this.ClientBounds.Height ? (this.PageSize.Width >= this.ClientBounds.Width || this.PageSize.Height >= this.ClientBounds.Height ? new Point(this.Margin.Left - this.hsb.Value, this.Margin.Top - this.vsb.Value) : new Point((this.ClientBounds.Width - this.PageSize.Width) / 2 + this.Margin.Left, (this.ClientBounds.Height - this.PageSize.Height) / 2 + this.Margin.Top)) : new Point(this.Margin.Left - this.hsb.Value, (this.ClientBounds.Height - this.PageSize.Height) / 2 + this.Margin.Top)) : new Point((this.ClientBounds.Width - this.PageSize.Width) / 2 + this.Margin.Left, this.Margin.Top - this.vsb.Value);
      this._pageBounds = new Rectangle(this._pageLocation, this.PageSize);
    }

    private void DoubleBufferControl_MarginChanged(object sender, EventArgs e)
    {
      this._clientLocation = new Point(this.Margin.Left, this.Margin.Top);
      this.Resized();
      this.Invalidate();
    }

    private void hsb_Scroll(object sender, ScrollEventArgs e)
    {
      if (!this.InvalidateScrollBarChanged)
        return;
      this.RecalcPageLocation();
      this.Invalidate();
    }

    private void vsb_Resize(object sender, EventArgs e)
    {
      this._scrollbarSize = new Size(this.vsb.Width, this.hsb.Height);
      this.Resized();
      this.Invalidate();
    }

    private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
    {
      if (!this.InvalidateScrollBarChanged)
        return;
      this.RecalcPageLocation();
      this.Invalidate();
    }

    private PageViewer.CursorStatus getCursorStatus(MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Middle)
        return PageViewer.CursorStatus.Move;
      return this._cursorStatus;
    }

    public bool MouseInPage(Point p)
    {
      if (this.IsActive)
        return new Rectangle(0, 0, this.CurrentView.Width, this.CurrentView.Height).Contains(p);
      return false;
    }

    private void DrawRubberFrame()
    {
      using (Graphics g = Graphics.FromHwndInternal(this.Handle))
        new GDI(g).DrawLine(Color.Gray, this._pointStart, this._pointCurrent);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      Point location = e.Location;
      if (this.MouseInPage(location) && this._bMouseCaptured)
      {
        switch (this.getCursorStatus(e))
        {
          case PageViewer.CursorStatus.Move:
            this.Cursor = Cursors.Hand;
            int num1 = e.X - this._pointCurrent.X;
            int num2 = e.Y - this._pointCurrent.Y;
            if (this.PageBounds.Width > this.ViewBounds.Width)
            {
              this._pointX = this.hsb.Value;
              this._pointX -= num1;
            }
            if (this.PageBounds.Height > this.ViewBounds.Height)
            {
              this._pointY = this.vsb.Value;
              this._pointY -= num2;
            }
            this.ScrollPosition = new Point(this._pointX, this._pointY);
            break;
          case PageViewer.CursorStatus.Zoom:
            this.Cursor = Cursors.SizeAll;
            int num3 = e.X - this._pointCurrent.X;
            int num4 = e.Y - this._pointCurrent.Y;
            this._pointCurrent = e.Location;
            this.DrawRubberFrame();
            break;
        }
      }
      if (!this.MouseInPage(location))
        return;
      this._pointCurrent = e.Location;
      e.Location.Offset(this.Margin.Top, this.Margin.Bottom);
      base.OnMouseMove(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      if (this.MouseInPage(e.Location) && e.Button == MouseButtons.Left)
      {
        this._pointCurrent = e.Location;
        this._pointStart = e.Location;
        this._bMouseCaptured = true;
      }
      e.Location.Offset(this.Margin.Top, this.Margin.Bottom);
      base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      if (!this.MouseInPage(e.Location) || !this._bMouseCaptured)
        return;
      if (this.getCursorStatus(e) == PageViewer.CursorStatus.Move)
        this.ScrollPosition = new Point(this._pointX, this._pointY);
      this.Cursor = Cursors.Default;
      this._bMouseCaptured = false;
      base.OnMouseUp(e);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
      if (this.MouseInPage(e.Location))
      {
        int y = this.ScrollPosition.Y;
        if (e.Delta < 0)
          y = this.ScrollPosition.Y + 120 * this.PageBounds.Height / this.ViewBounds.Height / 6;
        else if (e.Delta > 0)
          y = this.ScrollPosition.Y - 120 * this.PageBounds.Height / this.ViewBounds.Height / 6;
        this.ScrollPosition = new Point(this.ScrollPosition.X, y);
        if (this.vsb.Value == this.vsb.Maximum)
        {
          ++this._deltasCount;
          if (this._deltasCount > 1)
          {
            this._deltasCount = 0;
            if (this.NextPage != null)
              this.bHasMorePagesD = this.NextPage((object) this);
            this.bHasMorePagesT = true;
            if (this.bHasMorePagesD)
              this.ScrollPosition = new Point(this.hsb.Value, 0);
          }
        }
        if (this.vsb.Value == 0)
        {
          ++this._deltasCount;
          if (this._deltasCount > 1)
          {
            this._deltasCount = 0;
            if (this.PreviousPage != null)
              this.bHasMorePagesT = this.PreviousPage((object) this);
            this.bHasMorePagesD = true;
            if (this.bHasMorePagesT)
              this.ScrollPosition = new Point(this.hsb.Value, this.vsb.Maximum);
          }
        }
      }
      base.OnMouseWheel(e);
    }

    public delegate void PaintControlHandler(object sender, Graphics g);

    public delegate bool MovePageHandler(object sender);

    public enum CursorStatus
    {
      Select,
      Move,
      Zoom,
      Snapshot,
    }

    public enum DoubleBufferMethod
    {
      NoDoubleBuffer,
      BuiltInDoubleBuffer,
      BuiltInOptimizedDoubleBuffer,
      ManualDoubleBuffer11,
      ManualDoubleBuffer20,
    }
  }
}
