// Decompiled with JetBrains decompiler
// Type: Gma.UserActivityMonitor.HookManager
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
  public static class HookManager
  {
    private const int WH_MOUSE_LL = 14;
    private const int WH_KEYBOARD_LL = 13;
    private const int WH_MOUSE = 7;
    private const int WH_KEYBOARD = 2;
    private const int WM_MOUSEMOVE = 512;
    private const int WM_LBUTTONDOWN = 513;
    private const int WM_RBUTTONDOWN = 516;
    private const int WM_MBUTTONDOWN = 519;
    private const int WM_LBUTTONUP = 514;
    private const int WM_RBUTTONUP = 517;
    private const int WM_MBUTTONUP = 520;
    private const int WM_LBUTTONDBLCLK = 515;
    private const int WM_RBUTTONDBLCLK = 518;
    private const int WM_MBUTTONDBLCLK = 521;
    private const int WM_MOUSEWHEEL = 522;
    private const int WM_KEYDOWN = 256;
    private const int WM_KEYUP = 257;
    private const int WM_SYSKEYDOWN = 260;
    private const int WM_SYSKEYUP = 261;
    private const byte VK_SHIFT = 16;
    private const byte VK_CAPITAL = 20;
    private const byte VK_NUMLOCK = 144;
    private static MouseButtons s_PrevClickedButton;
    private static Timer s_DoubleClickTimer;
    private static HookManager.HookProc s_MouseDelegate;
    private static int s_MouseHookHandle;
    private static int m_OldX;
    private static int m_OldY;
    private static HookManager.HookProc s_KeyboardDelegate;
    private static int s_KeyboardHookHandle;

    private static event MouseEventHandler s_MouseMove;

    public static event MouseEventHandler MouseMove
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseMove += value;
      }
      remove
      {
        HookManager.s_MouseMove -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event EventHandler<MouseEventExtArgs> s_MouseMoveExt;

    public static event EventHandler<MouseEventExtArgs> MouseMoveExt
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseMoveExt += value;
      }
      remove
      {
        HookManager.s_MouseMoveExt -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event MouseEventHandler s_MouseClick;

    public static event MouseEventHandler MouseClick
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseClick += value;
      }
      remove
      {
        HookManager.s_MouseClick -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event EventHandler<MouseEventExtArgs> s_MouseClickExt;

    public static event EventHandler<MouseEventExtArgs> MouseClickExt
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseClickExt += value;
      }
      remove
      {
        HookManager.s_MouseClickExt -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event MouseEventHandler s_MouseDown;

    public static event MouseEventHandler MouseDown
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseDown += value;
      }
      remove
      {
        HookManager.s_MouseDown -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event MouseEventHandler s_MouseUp;

    public static event MouseEventHandler MouseUp
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseUp += value;
      }
      remove
      {
        HookManager.s_MouseUp -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event MouseEventHandler s_MouseWheel;

    public static event MouseEventHandler MouseWheel
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalMouseEvents();
        HookManager.s_MouseWheel += value;
      }
      remove
      {
        HookManager.s_MouseWheel -= value;
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event MouseEventHandler s_MouseDoubleClick;

    public static event MouseEventHandler MouseDoubleClick
    {
      add
      {
      }
      remove
      {
        if (HookManager.s_MouseDoubleClick != null)
        {
          HookManager.s_MouseDoubleClick -= value;
          if (HookManager.s_MouseDoubleClick == null)
          {
            HookManager.MouseUp -= new MouseEventHandler(HookManager.OnMouseUp);
            HookManager.s_DoubleClickTimer.Tick -= new EventHandler(HookManager.DoubleClickTimeElapsed);
            HookManager.s_DoubleClickTimer = (Timer) null;
          }
        }
        HookManager.TryUnsubscribeFromGlobalMouseEvents();
      }
    }

    private static event KeyPressEventHandler s_KeyPress;

    public static event KeyPressEventHandler KeyPress
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalKeyboardEvents();
        HookManager.s_KeyPress += value;
      }
      remove
      {
        HookManager.s_KeyPress -= value;
        HookManager.TryUnsubscribeFromGlobalKeyboardEvents();
      }
    }

    private static event KeyEventHandler s_KeyUp;

    public static event KeyEventHandler KeyUp
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalKeyboardEvents();
        HookManager.s_KeyUp += value;
      }
      remove
      {
        HookManager.s_KeyUp -= value;
        HookManager.TryUnsubscribeFromGlobalKeyboardEvents();
      }
    }

    private static event KeyEventHandler s_KeyDown;

    public static event KeyEventHandler KeyDown
    {
      add
      {
        HookManager.EnsureSubscribedToGlobalKeyboardEvents();
        HookManager.s_KeyDown += value;
      }
      remove
      {
        HookManager.s_KeyDown -= value;
        HookManager.TryUnsubscribeFromGlobalKeyboardEvents();
      }
    }

    private static void DoubleClickTimeElapsed(object sender, EventArgs e)
    {
      HookManager.s_DoubleClickTimer.Enabled = false;
      HookManager.s_PrevClickedButton = MouseButtons.None;
    }

    private static void OnMouseUp(object sender, MouseEventArgs e)
    {
      if (e.Clicks < 1)
        return;
      if (e.Button.Equals((object) HookManager.s_PrevClickedButton))
      {
        if (HookManager.s_MouseDoubleClick != null)
          HookManager.s_MouseDoubleClick((object) null, e);
        HookManager.s_DoubleClickTimer.Enabled = false;
        HookManager.s_PrevClickedButton = MouseButtons.None;
      }
      else
      {
        HookManager.s_DoubleClickTimer.Enabled = true;
        HookManager.s_PrevClickedButton = e.Button;
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int SetWindowsHookEx(int idHook, HookManager.HookProc lpfn, IntPtr hMod, int dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int UnhookWindowsHookEx(int idHook);

    [DllImport("user32")]
    public static extern int GetDoubleClickTime();

    [DllImport("user32")]
    private static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

    [DllImport("user32")]
    private static extern int GetKeyboardState(byte[] pbKeyState);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern short GetKeyState(int vKey);

    private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
    {
      if (nCode >= 0)
      {
        HookManager.MouseLLHookStruct structure = (HookManager.MouseLLHookStruct) Marshal.PtrToStructure(lParam, typeof (HookManager.MouseLLHookStruct));
        MouseButtons buttons = MouseButtons.None;
        short num = 0;
        int clicks = 0;
        bool flag1 = false;
        bool flag2 = false;
        switch (wParam)
        {
          case 513:
            flag1 = true;
            buttons = MouseButtons.Left;
            clicks = 1;
            break;
          case 514:
            flag2 = true;
            buttons = MouseButtons.Left;
            clicks = 1;
            break;
          case 515:
            buttons = MouseButtons.Left;
            clicks = 2;
            break;
          case 516:
            flag1 = true;
            buttons = MouseButtons.Right;
            clicks = 1;
            break;
          case 517:
            flag2 = true;
            buttons = MouseButtons.Right;
            clicks = 1;
            break;
          case 518:
            buttons = MouseButtons.Right;
            clicks = 2;
            break;
          case 522:
            num = (short) (structure.MouseData >> 16 & (int) ushort.MaxValue);
            break;
        }
        MouseEventExtArgs e = new MouseEventExtArgs(buttons, clicks, structure.Point.X, structure.Point.Y, (int) num);
        if (HookManager.s_MouseUp != null && flag2)
          HookManager.s_MouseUp((object) null, (MouseEventArgs) e);
        if (HookManager.s_MouseDown != null && flag1)
          HookManager.s_MouseDown((object) null, (MouseEventArgs) e);
        if (HookManager.s_MouseClick != null && clicks > 0)
          HookManager.s_MouseClick((object) null, (MouseEventArgs) e);
        if (HookManager.s_MouseClickExt != null && clicks > 0)
          HookManager.s_MouseClickExt((object) null, e);
        if (HookManager.s_MouseDoubleClick != null && clicks == 2)
          HookManager.s_MouseDoubleClick((object) null, (MouseEventArgs) e);
        if (HookManager.s_MouseWheel != null && (int) num != 0)
          HookManager.s_MouseWheel((object) null, (MouseEventArgs) e);
        if ((HookManager.s_MouseMove != null || HookManager.s_MouseMoveExt != null) && (HookManager.m_OldX != structure.Point.X || HookManager.m_OldY != structure.Point.Y))
        {
          HookManager.m_OldX = structure.Point.X;
          HookManager.m_OldY = structure.Point.Y;
          if (HookManager.s_MouseMove != null)
            HookManager.s_MouseMove((object) null, (MouseEventArgs) e);
          if (HookManager.s_MouseMoveExt != null)
            HookManager.s_MouseMoveExt((object) null, e);
        }
        if (e.Handled)
          return -1;
      }
      return HookManager.CallNextHookEx(HookManager.s_MouseHookHandle, nCode, wParam, lParam);
    }

    private static void EnsureSubscribedToGlobalMouseEvents()
    {
      if (HookManager.s_MouseHookHandle != 0)
        return;
      HookManager.s_MouseDelegate = new HookManager.HookProc(HookManager.MouseHookProc);
      HookManager.s_MouseHookHandle = HookManager.SetWindowsHookEx(14, HookManager.s_MouseDelegate, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
      if (HookManager.s_MouseHookHandle == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static void TryUnsubscribeFromGlobalMouseEvents()
    {
      if (HookManager.s_MouseClick != null || HookManager.s_MouseDown != null || (HookManager.s_MouseMove != null || HookManager.s_MouseUp != null) || (HookManager.s_MouseClickExt != null || HookManager.s_MouseMoveExt != null || HookManager.s_MouseWheel != null))
        return;
      HookManager.ForceUnsunscribeFromGlobalMouseEvents();
    }

    private static void ForceUnsunscribeFromGlobalMouseEvents()
    {
      if (HookManager.s_MouseHookHandle == 0)
        return;
      int num = HookManager.UnhookWindowsHookEx(HookManager.s_MouseHookHandle);
      HookManager.s_MouseHookHandle = 0;
      HookManager.s_MouseDelegate = (HookManager.HookProc) null;
      if (num == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
    {
      bool flag1 = false;
      if (nCode >= 0)
      {
        HookManager.KeyboardHookStruct structure = (HookManager.KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (HookManager.KeyboardHookStruct));
        if (HookManager.s_KeyDown != null && (wParam == 256 || wParam == 260))
        {
          KeyEventArgs e = new KeyEventArgs((Keys) structure.VirtualKeyCode);
          HookManager.s_KeyDown((object) null, e);
          flag1 = e.Handled;
        }
        if (HookManager.s_KeyPress != null && wParam == 256)
        {
          bool flag2 = ((int) HookManager.GetKeyState(16) & 128) == 128;
          bool flag3 = (int) HookManager.GetKeyState(20) != 0;
          byte[] numArray = new byte[256];
          HookManager.GetKeyboardState(numArray);
          byte[] lpwTransKey = new byte[2];
          if (HookManager.ToAscii(structure.VirtualKeyCode, structure.ScanCode, numArray, lpwTransKey, structure.Flags) == 1)
          {
            char upper = (char) lpwTransKey[0];
            if (flag3 ^ flag2 && char.IsLetter(upper))
              upper = char.ToUpper(upper);
            KeyPressEventArgs e = new KeyPressEventArgs(upper);
            HookManager.s_KeyPress((object) null, e);
            flag1 = flag1 || e.Handled;
          }
        }
        if (HookManager.s_KeyUp != null && (wParam == 257 || wParam == 261))
        {
          KeyEventArgs e = new KeyEventArgs((Keys) structure.VirtualKeyCode);
          HookManager.s_KeyUp((object) null, e);
          flag1 = flag1 || e.Handled;
        }
      }
      if (flag1)
        return -1;
      return HookManager.CallNextHookEx(HookManager.s_KeyboardHookHandle, nCode, wParam, lParam);
    }

    private static void EnsureSubscribedToGlobalKeyboardEvents()
    {
      if (HookManager.s_KeyboardHookHandle != 0)
        return;
      HookManager.s_KeyboardDelegate = new HookManager.HookProc(HookManager.KeyboardHookProc);
      HookManager.s_KeyboardHookHandle = HookManager.SetWindowsHookEx(13, HookManager.s_KeyboardDelegate, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
      if (HookManager.s_KeyboardHookHandle == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static void TryUnsubscribeFromGlobalKeyboardEvents()
    {
      if (HookManager.s_KeyDown != null || HookManager.s_KeyUp != null || HookManager.s_KeyPress != null)
        return;
      HookManager.ForceUnsunscribeFromGlobalKeyboardEvents();
    }

    private static void ForceUnsunscribeFromGlobalKeyboardEvents()
    {
      if (HookManager.s_KeyboardHookHandle == 0)
        return;
      int num = HookManager.UnhookWindowsHookEx(HookManager.s_KeyboardHookHandle);
      HookManager.s_KeyboardHookHandle = 0;
      HookManager.s_KeyboardDelegate = (HookManager.HookProc) null;
      if (num == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    private struct Point
    {
      public int X;
      public int Y;
    }

    private struct MouseLLHookStruct
    {
      public HookManager.Point Point;
      public int MouseData;
      public int Flags;
      public int Time;
      public int ExtraInfo;
    }

    private struct KeyboardHookStruct
    {
      public int VirtualKeyCode;
      public int ScanCode;
      public int Flags;
      public int Time;
      public int ExtraInfo;
    }
  }
}
