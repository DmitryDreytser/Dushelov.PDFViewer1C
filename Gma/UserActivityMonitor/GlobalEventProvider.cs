// Decompiled with JetBrains decompiler
// Type: Gma.UserActivityMonitor.GlobalEventProvider
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
  public class GlobalEventProvider : Component
  {
    protected override bool CanRaiseEvents
    {
      get
      {
        return true;
      }
    }

    private event MouseEventHandler m_MouseMove;

    public event MouseEventHandler MouseMove
    {
      add
      {
        if (this.m_MouseMove == null)
          HookManager.MouseMove += new MouseEventHandler(this.HookManager_MouseMove);
        this.m_MouseMove += value;
      }
      remove
      {
        this.m_MouseMove -= value;
        if (this.m_MouseMove != null)
          return;
        HookManager.MouseMove -= new MouseEventHandler(this.HookManager_MouseMove);
      }
    }

    private event MouseEventHandler m_MouseClick;

    public event MouseEventHandler MouseClick
    {
      add
      {
        if (this.m_MouseClick == null)
          HookManager.MouseClick += new MouseEventHandler(this.HookManager_MouseClick);
        this.m_MouseClick += value;
      }
      remove
      {
        this.m_MouseClick -= value;
        if (this.m_MouseClick != null)
          return;
        HookManager.MouseClick -= new MouseEventHandler(this.HookManager_MouseClick);
      }
    }

    private event MouseEventHandler m_MouseDown;

    public event MouseEventHandler MouseDown
    {
      add
      {
        if (this.m_MouseDown == null)
          HookManager.MouseDown += new MouseEventHandler(this.HookManager_MouseDown);
        this.m_MouseDown += value;
      }
      remove
      {
        this.m_MouseDown -= value;
        if (this.m_MouseDown != null)
          return;
        HookManager.MouseDown -= new MouseEventHandler(this.HookManager_MouseDown);
      }
    }

    private event MouseEventHandler m_MouseUp;

    public event MouseEventHandler MouseUp
    {
      add
      {
        if (this.m_MouseUp == null)
          HookManager.MouseUp += new MouseEventHandler(this.HookManager_MouseUp);
        this.m_MouseUp += value;
      }
      remove
      {
        this.m_MouseUp -= value;
        if (this.m_MouseUp != null)
          return;
        HookManager.MouseUp -= new MouseEventHandler(this.HookManager_MouseUp);
      }
    }

    private event MouseEventHandler m_MouseDoubleClick;

    public event MouseEventHandler MouseDoubleClick
    {
      add
      {
        if (this.m_MouseDoubleClick == null)
          HookManager.MouseDoubleClick += new MouseEventHandler(this.HookManager_MouseDoubleClick);
        this.m_MouseDoubleClick += value;
      }
      remove
      {
        this.m_MouseDoubleClick -= value;
        if (this.m_MouseDoubleClick != null)
          return;
        HookManager.MouseDoubleClick -= new MouseEventHandler(this.HookManager_MouseDoubleClick);
      }
    }

    private event EventHandler<MouseEventExtArgs> m_MouseMoveExt;

    public event EventHandler<MouseEventExtArgs> MouseMoveExt
    {
      add
      {
        if (this.m_MouseMoveExt == null)
          HookManager.MouseMoveExt += new EventHandler<MouseEventExtArgs>(this.HookManager_MouseMoveExt);
        this.m_MouseMoveExt += value;
      }
      remove
      {
        this.m_MouseMoveExt -= value;
        if (this.m_MouseMoveExt != null)
          return;
        HookManager.MouseMoveExt -= new EventHandler<MouseEventExtArgs>(this.HookManager_MouseMoveExt);
      }
    }

    private event EventHandler<MouseEventExtArgs> m_MouseClickExt;

    public event EventHandler<MouseEventExtArgs> MouseClickExt
    {
      add
      {
        if (this.m_MouseClickExt == null)
          HookManager.MouseClickExt += new EventHandler<MouseEventExtArgs>(this.HookManager_MouseClickExt);
        this.m_MouseClickExt += value;
      }
      remove
      {
        this.m_MouseClickExt -= value;
        if (this.m_MouseClickExt != null)
          return;
        HookManager.MouseClickExt -= new EventHandler<MouseEventExtArgs>(this.HookManager_MouseClickExt);
      }
    }

    private event KeyPressEventHandler m_KeyPress;

    public event KeyPressEventHandler KeyPress
    {
      add
      {
        if (this.m_KeyPress == null)
          HookManager.KeyPress += new KeyPressEventHandler(this.HookManager_KeyPress);
        this.m_KeyPress += value;
      }
      remove
      {
        this.m_KeyPress -= value;
        if (this.m_KeyPress != null)
          return;
        HookManager.KeyPress -= new KeyPressEventHandler(this.HookManager_KeyPress);
      }
    }

    private event KeyEventHandler m_KeyUp;

    public event KeyEventHandler KeyUp
    {
      add
      {
        if (this.m_KeyUp == null)
          HookManager.KeyUp += new KeyEventHandler(this.HookManager_KeyUp);
        this.m_KeyUp += value;
      }
      remove
      {
        this.m_KeyUp -= value;
        if (this.m_KeyUp != null)
          return;
        HookManager.KeyUp -= new KeyEventHandler(this.HookManager_KeyUp);
      }
    }

    private event KeyEventHandler m_KeyDown;

    public event KeyEventHandler KeyDown
    {
      add
      {
        if (this.m_KeyDown == null)
          HookManager.KeyDown += new KeyEventHandler(this.HookManager_KeyDown);
        this.m_KeyDown += value;
      }
      remove
      {
        this.m_KeyDown -= value;
        if (this.m_KeyDown != null)
          return;
        HookManager.KeyDown -= new KeyEventHandler(this.HookManager_KeyDown);
      }
    }

    private void HookManager_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.m_MouseMove == null)
        return;
      this.m_MouseMove((object) this, e);
    }

    private void HookManager_MouseClick(object sender, MouseEventArgs e)
    {
      if (this.m_MouseClick == null)
        return;
      this.m_MouseClick((object) this, e);
    }

    private void HookManager_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.m_MouseDown == null)
        return;
      this.m_MouseDown((object) this, e);
    }

    private void HookManager_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.m_MouseUp == null)
        return;
      this.m_MouseUp((object) this, e);
    }

    private void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.m_MouseDoubleClick == null)
        return;
      this.m_MouseDoubleClick((object) this, e);
    }

    private void HookManager_MouseMoveExt(object sender, MouseEventExtArgs e)
    {
      if (this.m_MouseMoveExt == null)
        return;
      this.m_MouseMoveExt((object) this, e);
    }

    private void HookManager_MouseClickExt(object sender, MouseEventExtArgs e)
    {
      if (this.m_MouseClickExt == null)
        return;
      this.m_MouseClickExt((object) this, e);
    }

    private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.m_KeyPress == null)
        return;
      this.m_KeyPress((object) this, e);
    }

    private void HookManager_KeyUp(object sender, KeyEventArgs e)
    {
      if (this.m_KeyUp == null)
        return;
      this.m_KeyUp((object) this, e);
    }

    private void HookManager_KeyDown(object sender, KeyEventArgs e)
    {
      this.m_KeyDown((object) this, e);
    }
  }
}
