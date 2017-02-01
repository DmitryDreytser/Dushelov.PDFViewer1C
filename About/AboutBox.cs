// Decompiled with JetBrains decompiler
// Type: About.AboutBox
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace About
{
  public class AboutBox : Form
  {
    private IContainer components;
    private string _emaillink;
    private LinkLabel emaillinklabel;
    private Label label2;
    private Label name;
    private Label version;
    private LinkLabel linkLabel1;

    public string emaillink
    {
      set
      {
        this._emaillink = value;
      }
    }

    public string programname
    {
      set
      {
        this.name.Text = value;
        this.alignecentre((Control) this.name);
      }
    }

    public AboutBox()
    {
      this.InitializeComponent();
      this._emaillink = "mailto:vasil@dushelov.ru";
    }

    public AboutBox(string title)
    {
      this.InitializeComponent();
      this.Text = title;
      this._emaillink = "mailto:vasil@dushelov.ru";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.name = new Label();
      this.version = new Label();
      this.emaillinklabel = new LinkLabel();
      this.label2 = new Label();
      this.linkLabel1 = new LinkLabel();
      this.SuspendLayout();
      this.name.AutoSize = true;
      this.name.BackColor = Color.Transparent;
      this.name.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.name.Location = new Point(116, 14);
      this.name.Name = "name";
      this.name.Size = new Size(41, 15);
      this.name.TabIndex = 6;
      this.name.Text = "label1";
      this.version.AutoSize = true;
      this.version.BackColor = Color.Transparent;
      this.version.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.version.Location = new Point(102, 35);
      this.version.Name = "version";
      this.version.Size = new Size(88, 15);
      this.version.TabIndex = 9;
      this.version.Text = "Version 1.0.0.0";
      this.emaillinklabel.AutoSize = true;
      this.emaillinklabel.BackColor = Color.Transparent;
      this.emaillinklabel.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.emaillinklabel.Location = new Point(219, 117);
      this.emaillinklabel.Name = "emaillinklabel";
      this.emaillinklabel.Size = new Size(46, 17);
      this.emaillinklabel.TabIndex = 8;
      this.emaillinklabel.TabStop = true;
      this.emaillinklabel.Text = "e-mail";
      this.emaillinklabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.emaillinklabel_LinkClicked);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label2.Location = new Point(10, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(236, 15);
      this.label2.TabIndex = 7;
      this.label2.Text = "Created and Developed by Dushelov 2009\r\n";
      this.label2.TextAlign = ContentAlignment.MiddleCenter;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.BackColor = Color.Transparent;
      this.linkLabel1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.linkLabel1.Location = new Point(79, 90);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(113, 17);
      this.linkLabel1.TabIndex = 11;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "www.dushelov.ru";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.AutoScaleDimensions = new SizeF(96f, 96f);
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.ClientSize = new Size(273, 147);
      this.Controls.Add((Control) this.linkLabel1);
      this.Controls.Add((Control) this.name);
      this.Controls.Add((Control) this.version);
      this.Controls.Add((Control) this.emaillinklabel);
      this.Controls.Add((Control) this.label2);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MaximumSize = new Size(300, 300);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(200, 169);
      this.Name = "AboutBox";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "AboutBox";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void alignecentre(Control con)
    {
      con.Location = new Point((this.Size.Width - 7 - con.Size.Width) / 2, con.Location.Y);
    }

    private void emaillinklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Process.Start(this._emaillink);
      }
      catch (Exception ex)
      {
      }
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Process.Start(this.linkLabel1.Text);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
