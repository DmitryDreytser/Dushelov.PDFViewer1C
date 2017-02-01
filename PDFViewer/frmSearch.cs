// Decompiled with JetBrains decompiler
// Type: PDFViewer.frmSearch
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Душелов;

namespace PDFViewer
{
  public class frmSearch : Form
  {
    private bool _fromBegin = true;
    private SearchPdfHandler _callback;
    private IContainer components;
    private TextBox txtSearch;
    private Label label1;
    private Button btnSearch;
    private Button btnSearchNext;
    private Button btnCancelSearch;
    private CheckBox chkWholeWord;
    private CheckBox chkSearchUp;
    private CheckBox chkFullSearch;

    public frmSearch(SearchPdfHandler callback)
    {
      this.InitializeComponent();
      this._callback = callback;
      this.Load += new EventHandler(this.frmSearch_Load);
      this.FormClosing += new FormClosingEventHandler(this.frmSearch_FormClosing);
    }

    private void frmSearch_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void frmSearch_Load(object sender, EventArgs e)
    {
    }

    private void HookManager_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        this.btnSearch.PerformClick();
      }
      else
      {
        if (e.KeyCode != Keys.F3)
          return;
        this.btnSearchNext.PerformClick();
      }
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if (this._callback == null)
        return;
      int num = this._callback(sender, new SearchArgs(this.txtSearch.Text, true, this.chkWholeWord.Checked, this.chkFullSearch.Checked, false, this.chkSearchUp.Checked));
    }

    private void btnSearchNext_Click(object sender, EventArgs e)
    {
      int num = 0;
      if (this._callback != null)
        num = this._callback(sender, new SearchArgs(this.txtSearch.Text, this._fromBegin, this.chkWholeWord.Checked, this.chkFullSearch.Checked, true, this.chkSearchUp.Checked));
      this._fromBegin = false;
      if (num != 0 || MessageBox.Show("Ничего не найдено. Искать сначала?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this._fromBegin = true;
      this.btnSearchNext.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmSearch));
      this.txtSearch = new TextBox();
      this.label1 = new Label();
      this.btnSearch = new Button();
      this.btnSearchNext = new Button();
      this.btnCancelSearch = new Button();
      this.chkWholeWord = new CheckBox();
      this.chkSearchUp = new CheckBox();
      this.chkFullSearch = new CheckBox();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.txtSearch, "txtSearch");
      this.txtSearch.Name = "txtSearch";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.btnSearch, "btnSearch");
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      componentResourceManager.ApplyResources((object) this.btnSearchNext, "btnSearchNext");
      this.btnSearchNext.Name = "btnSearchNext";
      this.btnSearchNext.UseVisualStyleBackColor = true;
      this.btnSearchNext.Click += new EventHandler(this.btnSearchNext_Click);
      this.btnCancelSearch.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancelSearch, "btnCancelSearch");
      this.btnCancelSearch.Name = "btnCancelSearch";
      this.btnCancelSearch.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkWholeWord, "chkWholeWord");
      this.chkWholeWord.Name = "chkWholeWord";
      this.chkWholeWord.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkSearchUp, "chkSearchUp");
      this.chkSearchUp.Name = "chkSearchUp";
      this.chkSearchUp.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.chkFullSearch, "chkFullSearch");
      this.chkFullSearch.Checked = true;
      this.chkFullSearch.CheckState = CheckState.Checked;
      this.chkFullSearch.Name = "chkFullSearch";
      this.chkFullSearch.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancelSearch;
      this.Controls.Add((Control) this.chkFullSearch);
      this.Controls.Add((Control) this.chkSearchUp);
      this.Controls.Add((Control) this.chkWholeWord);
      this.Controls.Add((Control) this.btnCancelSearch);
      this.Controls.Add((Control) this.btnSearchNext);
      this.Controls.Add((Control) this.btnSearch);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtSearch);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = "frmSearch";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
