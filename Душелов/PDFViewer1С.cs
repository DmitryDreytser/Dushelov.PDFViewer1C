// Decompiled with JetBrains decompiler
// Type: Душелов.PDFViewer1С
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using About;
using Gma.UserActivityMonitor;
using Microsoft.Win32;
using PDFLibNet;
using PDFViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using Душелов.Properties;

namespace Душелов
{
    [ProgId("Душелов.PDFViewer1С")]
    [ComSourceInterfaces(typeof(PDFViewer1С.IPDFViewerEvents))]
    [ClassInterface(ClassInterfaceType.None)]
    public class PDFViewer1С : UserControl, IPDFViewer1С, IDisposable
    {
        private Rectangle EmptyRectangle = new Rectangle(-1, -1, 0, 0);
        private PDFViewer1С.CursorStatus _cursorStatus = PDFViewer1С.CursorStatus.Move;
        private System.Drawing.Point _pointStart = System.Drawing.Point.Empty;
        private System.Drawing.Point _pointCurrent = System.Drawing.Point.Empty;
        private System.Drawing.Point _bMouseCapturedStart = System.Drawing.Point.Empty;
        private System.Drawing.Point EmptyPoint = new System.Drawing.Point(-1, -1);
        private System.Drawing.Point _lastPoint = new System.Drawing.Point(-1, -1);
        public static PDFViewer1С Instance;
        private bool _bMouseCaptured;
        private int _pointX;
        private int _pointY;
        private PDFWrapper _pdfDoc;
        private IContainer components;
        private TreeView tvwOutline;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbOpen;
        private ToolStripButton tsbPrev;
        private ToolStripTextBox txtPage;
        private ToolStripButton tsbNext;
        private ToolStripButton tsbSearch;
        private ToolStripButton tsbAbout;
        private ToolStripButton tsbPrint;
        private PrintDialog printDialog1;
        private PrintDocument printDocument1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolTip ttpLink;
        private ToolStripButton tsbPrintAs;
        private SplitContainer splitContainer1;
        private StatusStrip statusStrip1;
        public ToolStripStatusLabel StatusLabel;
        private PageViewer pageViewControl1;

        private bool IsActive
        {
            get
            {
                return PDFViewer1С.GetForegroundWindow() == this.Handle.ToInt32();
            }
        }

        public bool ВидимостьПанелиИнструментов
        {
            get
            {
                return this.toolStrip1.Visible;
            }
            set
            {
                this.toolStrip1.Visible = value;
            }
        }

        public bool ВидимостьОкнаЗакладок
        {
            get
            {
                return !this.splitContainer1.Panel1Collapsed;
            }
            set
            {
                this.splitContainer1.Panel1Collapsed = !value;
                this.Refresh();
            }
        }

        public int ТекущаяСтраница
        {
            get
            {
                if (this._pdfDoc != null)
                    return this._pdfDoc.CurrentPage;
                return 0;
            }
            set
            {
                if (this._pdfDoc == null || value <= 0 || value > this._pdfDoc.PageCount)
                    return;
                this._pdfDoc.CurrentPage = value;
                this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                this.Render();
            }
        }

        public int КоличествоСтраниц
        {
            get
            {
                if (this._pdfDoc != null)
                    return this._pdfDoc.PageCount;
                return 0;
            }
        }

        public double Масштаб
        {
            get
            {
                if (this._pdfDoc != null)
                    return this._pdfDoc.Zoom;
                return 0.0;
            }
            set
            {
                if (this._pdfDoc == null)
                    return;
                this._pdfDoc.Zoom = value;
                this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                this.Render();
            }
        }

        public event PDFViewer1С.OnPDFLoadCompeted ПослеЗагрузкиФайла;
        public event PDFViewer1С.OnPDFUnLoadCompeted ПослеВыгрузкиФайла;

        public PDFViewer1С()
        {
            this.InitializeComponent();
            PDFViewer1С.Instance = this;

            this.tvwOutline.BeforeExpand += new TreeViewCancelEventHandler(this.tvwOutline_BeforeExpand);
            this.tvwOutline.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvwOutline_NodeMouseClick);
            this.Resize += new EventHandler(this.frmPDFViewer_Resize);

            HookManager.MouseDown += new MouseEventHandler(this.HookManager_MouseDown);
            HookManager.MouseUp += new MouseEventHandler(this.HookManager_MouseUp);
            HookManager.MouseMove += new MouseEventHandler(this.HookManager_MouseMove);


        }

        [DllImport("user32.dll")]
        private static extern int GetForegroundWindow();

        [ComRegisterFunction]
        public static void RegisterClass(string key)
        {
            StringBuilder stringBuilder = new StringBuilder(key);
            stringBuilder.Replace("HKEY_CLASSES_ROOT\\", "");
            RegistryKey registryKey1 = Registry.ClassesRoot.OpenSubKey(stringBuilder.ToString(), true);
            if (registryKey1 == null)
                return;
            RegistryKey subKey = registryKey1.CreateSubKey("Control");
            if (subKey != null)
                subKey.Close();
            RegistryKey registryKey2 = registryKey1.OpenSubKey("InprocServer32", true);
            if (registryKey2 != null)
            {
                registryKey2.SetValue("CodeBase", (object)Assembly.GetExecutingAssembly().CodeBase);
                registryKey2.Close();
            }
            registryKey1.Close();
        }

        private void frmPDFViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Закрыть();
        }

        private PDFViewer1С.CursorStatus getCursorStatus(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                return PDFViewer1С.CursorStatus.Move;
            return this._cursorStatus;
        }

        private bool MouseInPage(System.Drawing.Point p)
        {
            if (this.IsActive)
                return this.pageViewControl1.MouseInPage(p);
            return false;
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            System.Drawing.Point client = this.pageViewControl1.PointToClient(e.Location);
            if (this._pdfDoc != null && (!this.MouseInPage(client) || !this._bMouseCaptured) && (this.MouseInPage(client) && this.getCursorStatus(e) == PDFViewer1С.CursorStatus.Move))
            {
                if (this.SearchLink(e.Location) != null)
                    this.pageViewControl1.Cursor = Cursors.Hand;
                else
                    this.pageViewControl1.Cursor = Cursors.Default;
            }
            this._pointCurrent = e.Location;
        }

        private PageLink SearchLink(System.Drawing.Point location)
        {
            System.Drawing.Point client = this.pageViewControl1.PointToClient(location);
            List<PageLink> links = this._pdfDoc.GetLinks(this._pdfDoc.CurrentPage);
            if (links != null)
            {
                foreach (PageLink pageLink in links)
                {
                    if (new Rectangle(this.pageViewControl1.PointUserToPage(System.Drawing.Point.Ceiling(this._pdfDoc.PointUserToDev((PointF)pageLink.Bounds.Location))), Size.Round(new SizeF((float)((double)pageLink.Bounds.Size.Width * this._pdfDoc.RenderDPI / 72.0), (float)((double)pageLink.Bounds.Size.Height * this._pdfDoc.RenderDPI / 72.0)))).Contains(client))
                        return pageLink;
                }
            }
            return (PageLink)null;
        }

        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.MouseInPage(this.pageViewControl1.PointToClient(e.Location)) && this._bMouseCaptured)
            {
                switch (this.getCursorStatus(e))
                {
                    case PDFViewer1С.CursorStatus.Zoom:
                        if (!this._pointCurrent.Equals((object)this.EmptyPoint))
                        {
                            if (e.Button == MouseButtons.Left && this._pdfDoc != null)
                            {
                                this._pdfDoc.ZoomIN();
                                break;
                            }
                            if (e.Button == MouseButtons.Right && this._pdfDoc != null)
                            {
                                this._pdfDoc.ZoomOut();
                                break;
                            }
                            break;
                        }
                        break;
                }
                this.pageViewControl1.Cursor = Cursors.Default;
            }
            this.ReleaseRubberFrame();
            this._bMouseCaptured = false;
        }

        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.MouseInPage(this.pageViewControl1.PointToClient(e.Location)) || e.Button != MouseButtons.Left)
                return;
            PageLink pageLink = this.SearchLink(e.Location);
            if (pageLink != null)
            {
                switch (pageLink.Action.Kind)
                {
                    case LinkActionKind.actionGoTo:
                        PageLinkGoTo action1 = pageLink.Action as PageLinkGoTo;
                        if (action1.Destination != null)
                        {
                            this._pdfDoc.CurrentPage = action1.Destination.Page;
                            PointF dev = this._pdfDoc.PointUserToDev(new PointF((float)action1.Destination.Left, (float)action1.Destination.Top));
                            if (action1.Destination.ChangeTop)
                                this.ScrolltoTop((int)dev.Y);
                            else
                                this.ScrolltoTop(0);
                            this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                            this.Render();
                            break;
                        }
                        if (action1.DestinationName != null)
                            break;
                        break;
                    case LinkActionKind.actionURI:
                        PageLinkURI action2 = pageLink.Action as PageLinkURI;
                        if (MessageBox.Show("Launching external application" + Environment.NewLine + action2.URL, this.Text, MessageBoxButtons.OKCancel) != DialogResult.OK)
                            break;
                        new Process()
                        {
                            StartInfo = {
                FileName = PDFViewer1С.GetDefaultBrowserPath(),
                Arguments = action2.URL
              }
                        }.Start();
                        break;
                }
            }
            else
            {
                this._pointCurrent = e.Location;
                this._pointStart = e.Location;
                this._bMouseCaptured = true;
            }
        }

        private void frmPDFViewer_Resize(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            this.FitWidth();
            this.Render();
        }

        private void Render()
        {
            this.pageViewControl1.PageSize = new Size(this._pdfDoc.PageWidth, this._pdfDoc.PageHeight);
            this.txtPage.Text = string.Format("{0}/{1}", (object)this._pdfDoc.CurrentPage, (object)this._pdfDoc.PageCount);
        }

        private void FitWidth()
        {
            using (PictureBox pictureBox = new PictureBox())
            {
                pictureBox.Width = this.pageViewControl1.ClientSize.Width;
                this._pdfDoc.FitToWidth(pictureBox.Handle);
            }
            this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
        }

        private void FitHeight()
        {
            using (PictureBox pictureBox = new PictureBox())
            {
                pictureBox.Width = this.pageViewControl1.ClientSize.Height;
                this._pdfDoc.FitToHeight(pictureBox.Handle);
            }
            this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
            this.pageViewControl1.Height = this._pdfDoc.PageHeight;
        }

        private void FillTree()
        {
            this.tvwOutline.Nodes.Clear();
            foreach (OutlineItem outlineItem in this._pdfDoc.Outline)
            {
                TreeNode node = new TreeNode(outlineItem.Title);
                node.Tag = (object)outlineItem;
                if (outlineItem.KidsCount > 0)
                    node.Nodes.Add(new TreeNode("dummy"));
                this.tvwOutline.Nodes.Add(node);
            }
        }

        private void tvwOutline_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OutlineItem tag = (OutlineItem)e.Node.Tag;
            if (tag == null)
                return;
            tag.DoAction();
            switch (tag.GetKind())
            {
                case LinkActionKind.actionGoTo:
                case LinkActionKind.actionGoToR:
                    PointF dev = this._pdfDoc.PointUserToDev(new PointF((float)tag.Destination.Left, (float)tag.Destination.Top));
                    if (tag.Destination.ChangeTop)
                    {
                        this.ScrolltoTop((int)dev.Y);
                        break;
                    }
                    this.ScrolltoTop(0);
                    break;
            }
            this.FitWidth();
            this.Render();
        }

        private void tvwOutline_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            OutlineItem tag = (OutlineItem)e.Node.Tag;
            if (tag == null || e.Node.Nodes.Count <= 0 || !(e.Node.Nodes[0].Text == "dummy"))
                return;
            e.Node.Nodes.Clear();
            foreach (OutlineItem children in (IEnumerable<OutlineItem>)tag.Childrens)
            {
                TreeNode node = new TreeNode(children.Title);
                node.Tag = (object)children;
                if (children.KidsCount > 0)
                    node.Nodes.Add(new TreeNode("dummy"));
                e.Node.Nodes.Add(node);
            }
        }

        private void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (this._pdfDoc == null || e.KeyCode != Keys.Return)
                    return;
                int result = -1;
                if (!int.TryParse(this.txtPage.Text, out result))
                    return;
                this.ТекущаяСтраница = result;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "Error");
            }
        }

        private void tsbNext_Click(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            this._pdfDoc.NextPage();
            this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
            this.Render();
        }

        private void tsbPrev_Click(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            this._pdfDoc.PreviousPage();
            this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
            this.Render();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            this._pdfDoc = (PDFWrapper)null;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Portable Document Format (*.pdf)|*.pdf";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                this.pageViewControl1.Visible = false;

                if (!this.LoadFile(openFileDialog.FileName))
                    return;

            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.ToString());
            }
        }

        private bool LoadFile(string filename)
        {
            try
            {
                this.pageViewControl1.Visible = false;
                if (this._pdfDoc == null)
                {
                    this._pdfDoc = new PDFWrapper();
                    this._pdfDoc.PDFLoadCompeted += new PDFLoadCompletedHandler(this._pdfDoc_PDFLoadCompeted);
                    this._pdfDoc.PDFLoadBegin += new PDFLoadBeginHandler(this._pdfDoc_PDFLoadBegin);
                }
                return this._pdfDoc.LoadPDF(filename);
            }
            catch (SecurityException ex)
            {
                frmPassword frmPassword = new frmPassword();
                if (frmPassword.ShowDialog() == DialogResult.OK)
                {
                    if (!frmPassword.UserPassword.Equals(string.Empty))
                        this._pdfDoc.UserPassword = frmPassword.UserPassword;
                    if (!frmPassword.OwnerPassword.Equals(string.Empty))
                        this._pdfDoc.OwnerPassword = frmPassword.OwnerPassword;
                    return this.LoadFile(filename);
                }
                int num = (int)MessageBox.Show("File encrypted", this.Text);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
            finally
            {

            }

        }

        private void CloseFile()
        {
            if (this._pdfDoc == null)
                return;

            this._pdfDoc.PDFLoadCompeted -= new PDFLoadCompletedHandler(this._pdfDoc_PDFLoadCompeted);
            this._pdfDoc.PDFLoadBegin -= new PDFLoadBeginHandler(this._pdfDoc_PDFLoadBegin);

            this._pdfDoc.Dispose();
            this._pdfDoc = null;

            ПослеВыгрузкиФайла?.Invoke();
        }

        private void _pdfDoc_PDFLoadBegin()
        {
            this.UpdateParamsUI(false);

        }

        private void _pdfDoc_PDFLoadCompeted()
        {


            this._pdfDoc.CurrentPage = 1;
            this.Text = "Powered by xPDF: " + this._pdfDoc.Author + " - " + this._pdfDoc.Title;
            this.FillTree();
            this.FitWidth();
            this.Render();
            this.pageViewControl1.PageSize = new Size(this._pdfDoc.PageWidth, this._pdfDoc.PageHeight);
            this.pageViewControl1.Visible = true;



            this.UpdateParamsUI();
            if (this.ПослеЗагрузкиФайла == null)
                return;
            this.ПослеЗагрузкиФайла();
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox("Просмотр PDF в 1С");
            aboutBox.programname = "Просмотр PDF в 1С";
            aboutBox.StartPosition = FormStartPosition.CenterScreen;
            aboutBox.BackColor = System.Drawing.Color.SandyBrown;
            int num = (int)aboutBox.ShowDialog();
        }

        private int SearchCallBack(object sender, SearchArgs e)
        {
            int num = 0;
            if (this._pdfDoc != null)
            {
                this._pdfDoc.SearchCaseSensitive = e.Exact;
                if (this._pdfDoc.SearchResults.Count == 0 && e.FindNext)
                {
                    e.FindNext = false;
                    e.FromBegin = true;
                }

                num = 0;
                if (!e.FromBegin)
                {
                    if (!e.FindNext)
                    {
                        num = this._pdfDoc.FindText(e.Text, this._pdfDoc.CurrentPage, e.WholeDoc ? PDFSearchOrder.PDFSearchFromdBegin : PDFSearchOrder.PDFSearchFromCurrent, e.Exact, e.Up, true, e.WholeDoc);
                    }
                    else
                    {
                        if (!e.Up)
                        {
                            num = this._pdfDoc.FindNext(e.Text);
                        }
                        else
                        {
                            num = this._pdfDoc.FindPrevious(e.Text);
                        }
                    }

                }
                else
                    num = this._pdfDoc.FindFirst(e.Text, e.WholeDoc ? PDFSearchOrder.PDFSearchFromdBegin : PDFSearchOrder.PDFSearchFromCurrent, e.Up);

                if (num > 0)
                {
                    this._pdfDoc.CurrentPage = this._pdfDoc.SearchResults[0].Page;
                    this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                    this.FocusSearchResult(this._pdfDoc.SearchResults[0]);
                    this.Render();
                }
            }
            return num;
        }

        private void FocusSearchResult(PDFSearchResult res)
        {
            System.Drawing.Point scrollPosition = this.pageViewControl1.ScrollPosition;
            if (this._pdfDoc.PageWidth > this.pageViewControl1.Width)
                scrollPosition.X = res.Position.Left - this.pageViewControl1.Width - this.pageViewControl1.Margin.Size.Width;
            if (this._pdfDoc.PageHeight > this.pageViewControl1.Height)
                scrollPosition.Y = res.Position.Top - this.pageViewControl1.Height - this.pageViewControl1.Margin.Size.Height;
            this.pageViewControl1.ScrollPosition = scrollPosition;
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            try
            {
                int num = (int)new frmSearch(new SearchPdfHandler(this.SearchCallBack)).ShowDialog();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void ScrolltoTop(int y)
        {
            System.Drawing.Point scrollPosition = this.pageViewControl1.ScrollPosition;
            if (this._pdfDoc.PageHeight > this.pageViewControl1.Height)
                scrollPosition.Y = y;
            this.pageViewControl1.ScrollPosition = scrollPosition;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            try
            {
                string str = Path.GetTempPath() + Guid.NewGuid().ToString() + ".ps";
                if (this.printDialog1.ShowDialog() != DialogResult.OK)
                    return;
                if (this.printDialog1.PrinterSettings.PrintToFile)
                {
                    this.saveFileDialog1.Filter = "PostScript File (*.ps)|*.ps";
                    if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
                        return;
                    this._pdfDoc.PrintToFile(this.saveFileDialog1.FileName, 1, this._pdfDoc.PageCount);
                }
                else
                {
                    this._pdfDoc.PrintToFile(str, 1, this._pdfDoc.PageCount);
                    RawPrinterHelper.SendFileToPrinter(this.printDialog1.PrinterSettings.PrinterName, str);
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._pdfDoc == null)
                    return;
                this._pdfDoc.ZoomIN();
                this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                this.Render();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void tsbZoomOut_Click(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            this._pdfDoc.ZoomOut();
            this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
            this.Render();
        }

        private void DrawRubberFrame()
        {
            if ((!this._lastPoint.Equals((object)this.EmptyPoint) || this._bMouseCaptured && !this._pointCurrent.Equals((object)this.EmptyPoint)) && !this._lastPoint.Equals((object)this.EmptyPoint))
                this.ReleaseRubberFrame();
            this._lastPoint = this._pointCurrent;
            this.DrawRubberFrame(this._pointStart, this._pointCurrent);
        }

        private void ReleaseRubberFrame()
        {
            if (!this._lastPoint.Equals((object)this.EmptyPoint))
                this.DrawRubberFrame(this._pointStart, this._lastPoint);
            this._lastPoint = this.EmptyPoint;
        }

        private void DrawRubberFrame(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            Rectangle rectangle = new Rectangle();
            if (p1.X < p2.X)
            {
                rectangle.X = p1.X;
                rectangle.Width = p2.X - p1.X;
            }
            else
            {
                rectangle.X = p2.X;
                rectangle.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                rectangle.Y = p1.Y;
                rectangle.Height = p2.Y - p1.Y;
            }
            else
            {
                rectangle.Y = p2.Y;
                rectangle.Height = p1.Y - p2.Y;
            }
            ControlPaint.DrawReversibleFrame(rectangle, System.Drawing.Color.Gray, FrameStyle.Dashed);
        }

        private static string GetDefaultBrowserPath()
        {
            string name = "htmlfile\\shell\\open\\command";
            return ((string)Registry.ClassesRoot.OpenSubKey(name, false).GetValue((string)null, (object)null)).Split('"')[1];
        }

        private void tsbPrintAs_Click(object sender, EventArgs e)
        {
            if (this._pdfDoc == null)
                return;
            try
            {
                this.saveFileDialog1.Filter = "PostScript file (*.ps)|*.ps|Plain text (*.txt)|*.txt|Jpg Image (*.jpg)|*.jpg";
                if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                if (this.saveFileDialog1.FileName.EndsWith(".ps"))
                    this._pdfDoc.PrintToFile(this.saveFileDialog1.FileName, 1, this._pdfDoc.PageCount);
                else if (this.saveFileDialog1.FileName.EndsWith(".jpg"))
                    this._pdfDoc.ExportJpg(this.saveFileDialog1.FileName, 70);
                else if (this.saveFileDialog1.FileName.EndsWith(".txt"))
                {
                    this._pdfDoc.ExportText(this.saveFileDialog1.FileName, 1, this._pdfDoc.PageCount, true, true);
                }
                else
                {
                    if (!this.saveFileDialog1.FileName.EndsWith(".html"))
                        return;
                    this._pdfDoc.ExportHtml(this.saveFileDialog1.FileName, 1, this._pdfDoc.PageCount, false, false, true);
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.ToString());
            }
        }

        private void doubleBufferControl1_PaintControl(object sender, Graphics g)
        {
            if (this._pdfDoc == null)
                return;
            this._pdfDoc.ClientBounds = new Rectangle(this.pageViewControl1.PageLocation, this.pageViewControl1.CurrentView.Size);
            this._pdfDoc.CurrentX = this.pageViewControl1.CurrentView.X;
            this._pdfDoc.CurrentY = this.pageViewControl1.CurrentView.Y;
            this._pdfDoc.DrawPageHDC(g.GetHdc());
            g.ReleaseHdc();
        }

        private bool doubleBufferControl1_NextPage(object sender)
        {
            try
            {
                if (this._pdfDoc.CurrentPage < this._pdfDoc.PageCount)
                {
                    this._pdfDoc.NextPage();
                    this.Render();
                    this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                    return true;
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.ToString());
            }
            return false;
        }

        private bool doubleBufferControl1_PreviousPage(object sender)
        {
            try
            {
                if (this._pdfDoc.CurrentPage > 1)
                {
                    this._pdfDoc.PreviousPage();
                    this.Render();
                    this._pdfDoc.RenderPage(this.pageViewControl1.Handle);
                    return true;
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.ToString());
            }
            return false;
        }

        private void UpdateParamsUI()
        {
            this.UpdateParamsUI(true);
        }

        private void UpdateParamsUI(bool enabled)
        {
        }

        public void Закрыть()
        {
            this.CloseFile();
        }

        public bool ЗагрузитьДокумент(string Файл)
        {
            return this.LoadFile(Файл);
        }

        public int Найти(string СтрокаПоиска, bool СНачала, bool ТочноеСовпадение, bool ПоВсемуДокументу, bool Следующий, bool ПоискВверх)
        {
            return SearchCallBack(this, new SearchArgs(СтрокаПоиска, СНачала, ТочноеСовпадение, ПоВсемуДокументу, Следующий, ПоискВверх));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();


            this.tvwOutline.BeforeExpand -= new TreeViewCancelEventHandler(this.tvwOutline_BeforeExpand);
            this.tvwOutline.NodeMouseClick -= new TreeNodeMouseClickEventHandler(this.tvwOutline_NodeMouseClick);
            this.Resize -= new EventHandler(this.frmPDFViewer_Resize);
            HookManager.MouseDown -= new MouseEventHandler(this.HookManager_MouseDown);
            HookManager.MouseUp -= new MouseEventHandler(this.HookManager_MouseUp);
            HookManager.MouseMove -= new MouseEventHandler(this.HookManager_MouseMove);
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PDFViewer1С));
            this.splitContainer1 = new SplitContainer();
            this.tvwOutline = new TreeView();
            this.toolStrip1 = new ToolStrip();
            this.tsbOpen = new ToolStripButton();
            this.tsbPrintAs = new ToolStripButton();
            this.tsbPrev = new ToolStripButton();
            this.txtPage = new ToolStripTextBox();
            this.tsbNext = new ToolStripButton();
            this.tsbSearch = new ToolStripButton();
            this.tsbPrint = new ToolStripButton();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripButton2 = new ToolStripButton();
            this.tsbAbout = new ToolStripButton();
            this.printDialog1 = new PrintDialog();
            this.printDocument1 = new PrintDocument();
            this.saveFileDialog1 = new SaveFileDialog();
            this.ttpLink = new ToolTip(this.components);
            this.statusStrip1 = new StatusStrip();
            this.StatusLabel = new ToolStripStatusLabel();
            this.pageViewControl1 = new PageViewer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            componentResourceManager.ApplyResources((object)this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add((Control)this.tvwOutline);
            componentResourceManager.ApplyResources((object)this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel2.Controls.Add((Control)this.pageViewControl1);
            componentResourceManager.ApplyResources((object)this.splitContainer1.Panel2, "splitContainer1.Panel2");
            componentResourceManager.ApplyResources((object)this.tvwOutline, "tvwOutline");
            this.tvwOutline.Name = "tvwOutline";
            this.toolStrip1.Items.AddRange(new ToolStripItem[10]
            {
        (ToolStripItem) this.tsbOpen,
        (ToolStripItem) this.tsbPrintAs,
        (ToolStripItem) this.tsbPrev,
        (ToolStripItem) this.txtPage,
        (ToolStripItem) this.tsbNext,
        (ToolStripItem) this.tsbSearch,
        (ToolStripItem) this.tsbPrint,
        (ToolStripItem) this.toolStripButton1,
        (ToolStripItem) this.toolStripButton2,
        (ToolStripItem) this.tsbAbout
            });
            componentResourceManager.ApplyResources((object)this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.tsbOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = (Image)Resources.dmdskres_373_9_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbOpen, "tsbOpen");
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Click += new EventHandler(this.tsbOpen_Click);
            this.tsbPrintAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPrintAs.Image = (Image)Resources.PrintBrmUi_102_6_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbPrintAs, "tsbPrintAs");
            this.tsbPrintAs.Name = "tsbPrintAs";
            this.tsbPrintAs.Click += new EventHandler(this.tsbPrintAs_Click);
            this.tsbPrev.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPrev.Image = (Image)Resources.netshell_21611_1_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbPrev, "tsbPrev");
            this.tsbPrev.Name = "tsbPrev";
            this.tsbPrev.Click += new EventHandler(this.tsbPrev_Click);
            this.txtPage.Name = "txtPage";
            componentResourceManager.ApplyResources((object)this.txtPage, "txtPage");
            this.txtPage.KeyDown += new KeyEventHandler(this.txtPage_KeyDown);
            this.tsbNext.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbNext.Image = (Image)Resources.netshell_1611_1_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbNext, "tsbNext");
            this.tsbNext.Name = "tsbNext";
            this.tsbNext.Click += new EventHandler(this.tsbNext_Click);
            this.tsbSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbSearch.Image = (Image)Resources.SearchFolder_323_3_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbSearch, "tsbSearch");
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Click += new EventHandler(this.tsbSearch_Click);
            this.tsbPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbPrint.Image = (Image)Resources.FeedbackTool_109_12_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbPrint, "tsbPrint");
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = (Image)Resources.ZoomIn;
            componentResourceManager.ApplyResources((object)this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new EventHandler(this.tsbZoomIn_Click);
            this.toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = (Image)Resources.ZoomOut;
            componentResourceManager.ApplyResources((object)this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new EventHandler(this.tsbZoomOut_Click);
            this.tsbAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = (Image)Resources.psr_206_4_16x16x32;
            componentResourceManager.ApplyResources((object)this.tsbAbout, "tsbAbout");
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Click += new EventHandler(this.tsbAbout_Click);
            this.printDialog1.UseEXDialog = true;
            this.statusStrip1.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) this.StatusLabel
            });
            componentResourceManager.ApplyResources((object)this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.StatusLabel.Name = "StatusLabel";
            componentResourceManager.ApplyResources((object)this.StatusLabel, "StatusLabel");
            this.pageViewControl1.BackColor = SystemColors.AppWorkspace;
            this.pageViewControl1.BorderColor = System.Drawing.Color.Black;
            componentResourceManager.ApplyResources((object)this.pageViewControl1, "pageViewControl1");
            this.pageViewControl1.DrawBorder = false;
            this.pageViewControl1.DrawShadow = true;
            this.pageViewControl1.Name = "pageViewControl1";
            this.pageViewControl1.PageColor = SystemColors.AppWorkspace;
            this.pageViewControl1.PageSize = new Size(0, 0);
            this.pageViewControl1.PaintMethod = PageViewer.DoubleBufferMethod.BuiltInOptimizedDoubleBuffer;
            this.pageViewControl1.ScrollPosition = new System.Drawing.Point(-10, -10);
            this.pageViewControl1.PreviousPage += new PageViewer.MovePageHandler(this.doubleBufferControl1_PreviousPage);
            this.pageViewControl1.NextPage += new PageViewer.MovePageHandler(this.doubleBufferControl1_NextPage);
            this.pageViewControl1.PaintControl += new PageViewer.PaintControlHandler(this.doubleBufferControl1_PaintControl);
            componentResourceManager.ApplyResources((object)this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add((Control)this.statusStrip1);
            this.Controls.Add((Control)this.splitContainer1);
            this.Controls.Add((Control)this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "PDFViewer1С";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public enum CursorStatus
        {
            Select,
            Move,
            Zoom,
            Snapshot,
        }

        public delegate void OnPDFLoadCompeted();
        public delegate void OnPDFUnLoadCompeted();

        [Guid("E3310268-8E35-417e-9C00-252C5FB24265")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IPDFViewerEvents
        {
            [DispId(1610743808)]
            void ПослеЗагрузкиФайла();
            [DispId(1610743809)]
            void ПослеВыгрузкиФайла();
        }
    }
}
