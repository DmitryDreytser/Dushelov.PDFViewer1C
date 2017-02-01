// Decompiled with JetBrains decompiler
// Type: PDFViewer.frmPassword
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PDFViewer
{
  public class frmPassword : Form
  {
    private IContainer components;
    private TextBox txtUserPwd;
    private TextBox txtOwnPwd;
    private Label label1;
    private Label label2;
    private Button cmdAccept;
    private Button cmCancel;

    public string UserPassword
    {
      get
      {
        return this.txtUserPwd.Text;
      }
    }

    public string OwnerPassword
    {
      get
      {
        return this.txtOwnPwd.Text;
      }
    }

    public frmPassword()
    {
      this.InitializeComponent();
    }

    private void cmdAccept_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtUserPwd = new TextBox();
      this.txtOwnPwd = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.cmdAccept = new Button();
      this.cmCancel = new Button();
      this.SuspendLayout();
      this.txtUserPwd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtUserPwd.Location = new Point(92, 12);
      this.txtUserPwd.Name = "txtUserPwd";
      this.txtUserPwd.PasswordChar = '*';
      this.txtUserPwd.Size = new Size(158, 20);
      this.txtUserPwd.TabIndex = 0;
      this.txtOwnPwd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOwnPwd.Location = new Point(92, 39);
      this.txtOwnPwd.Name = "txtOwnPwd";
      this.txtOwnPwd.PasswordChar = '*';
      this.txtOwnPwd.Size = new Size(158, 20);
      this.txtOwnPwd.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Пользователь:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(59, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Владелец:";
      this.cmdAccept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmdAccept.Location = new Point(92, 65);
      this.cmdAccept.Name = "cmdAccept";
      this.cmdAccept.Size = new Size(75, 23);
      this.cmdAccept.TabIndex = 4;
      this.cmdAccept.Text = "&ОК";
      this.cmdAccept.UseVisualStyleBackColor = true;
      this.cmdAccept.Click += new EventHandler(this.cmdAccept_Click);
      this.cmCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmCancel.DialogResult = DialogResult.Cancel;
      this.cmCancel.Location = new Point(173, 65);
      this.cmCancel.Name = "cmCancel";
      this.cmCancel.Size = new Size(75, 23);
      this.cmCancel.TabIndex = 5;
      this.cmCancel.Text = "&Отмена";
      this.cmCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.cmdAccept;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cmCancel;
      this.ClientSize = new Size(260, 96);
      this.ControlBox = false;
      this.Controls.Add((Control) this.cmCancel);
      this.Controls.Add((Control) this.cmdAccept);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtOwnPwd);
      this.Controls.Add((Control) this.txtUserPwd);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = "frmPassword";
      this.Text = "Пользователь/Владелец (пароль)";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
