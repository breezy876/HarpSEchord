using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using static Common.FFXIV.FFXIVKeybindDat;

namespace Common.Interop
{

    public class GameKeyboardHook
    {
        #region pinvoke details

        #region Windows constants

        //values from Winuser.h in Microsoft SDK.
        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
        /// </summary>
        private const int WH_MOUSE_LL = 14;

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard  input events.
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure. 
        /// </summary>
        private const int WH_MOUSE = 7;

        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure. 
        /// </summary>
        private const int WH_KEYBOARD = 2;

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. 
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button 
        /// </summary>
        private const int WM_LBUTTONDOWN = 0x201;

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button
        /// </summary>
        private const int WM_RBUTTONDOWN = 0x204;

        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button 
        /// </summary>
        private const int WM_MBUTTONDOWN = 0x207;

        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button 
        /// </summary>
        private const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button 
        /// </summary>
        private const int WM_RBUTTONUP = 0x205;

        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button 
        /// </summary>
        private const int WM_MBUTTONUP = 0x208;

        /// <summary>
        /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button 
        /// </summary>
        private const int WM_LBUTTONDBLCLK = 0x203;

        /// <summary>
        /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button 
        /// </summary>
        private const int WM_RBUTTONDBLCLK = 0x206;

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button 
        /// </summary>
        private const int WM_MBUTTONDBLCLK = 0x209;

        /// <summary>
        /// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel. 
        /// </summary>
        private const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
        /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        private const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
        /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
        /// or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        private const int WM_KEYUP = 0x101;

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
        /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
        /// presses another key. It also occurs when no window currently has the keyboard focus; 
        /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
        /// receives the message can distinguish between these two contexts by checking the context 
        /// code in the lParam parameter. 
        /// </summary>
        private const int WM_SYSKEYDOWN = 0x104;

        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
        /// releases a key that was pressed while the ALT key was held down. It also occurs when no 
        /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent 
        /// to the active window. The window that receives the message can distinguish between 
        /// these two contexts by checking the context code in the lParam parameter. 
        /// </summary>
        private const int WM_SYSKEYUP = 0x105;

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;
        #endregion

        private enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        private struct keyboardHookStruct
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr extraInfo;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        [DllImport("USER32.dll")]
        static extern short GetKeyState(int key);


        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(
            HookType code, HookProc func, IntPtr instance, int threadID);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(
            IntPtr hook, int code, IntPtr wParam, ref keyboardHookStruct lParam);

        #endregion

        HookType hookType = HookType.WH_KEYBOARD_LL;
        IntPtr hookHandle = IntPtr.Zero;
        HookProc hookProc = null;

        // hook method called by system
        private delegate int HookProc(int code, IntPtr wParam, ref keyboardHookStruct lParam);


        public GameKeyboardHook()
        {
            WindowHandles = new List<IntPtr>();
            hookProc = new HookProc(HookCallback);
        }

        ~GameKeyboardHook()
        {
            Uninstall();
        }

        // events
        public delegate void HookEventHandler(object sender, GameKeyboardHookEventArgs e);

        public event HookEventHandler KeyDown;
        public event HookEventHandler KeyUp;

        public bool Installed { get; private set; }

        public bool PreventInput { get; set; }

        public List<IntPtr> WindowHandles { get; set; }

        // hook function called by system
        private int HookCallback(int code, IntPtr wParam, ref keyboardHookStruct lParam)
        {
            var foregroundWindowHandle = WindowHelpers.GetForegroundWindow();

            if (code < 0 || !WindowHandles.Contains(foregroundWindowHandle))
               return CallNextHookEx(hookHandle, code, wParam, ref lParam);

             if (this.KeyDown != null && wParam.ToInt32() == WM_KEYDOWN)
            {

                if (lParam.extraInfo == new IntPtr(0x8000))
                    return CallNextHookEx(hookHandle, code, wParam, ref lParam);

                this.KeyDown(this, new GameKeyboardHookEventArgs(lParam.vkCode, foregroundWindowHandle));
            }

            if (this.KeyUp != null && wParam.ToInt32() == WM_KEYUP)
            {

                if (lParam.extraInfo == new IntPtr(0x8000))
                    return CallNextHookEx(hookHandle, code, wParam, ref lParam);

                //bool ctrlPressed = !(IsKeyDown(Keys.LControlKey) || IsKeyDown(Keys.RControlKey) || (Keys)lParam.vkCode == Keys.LControlKey || (Keys)lParam.vkCode == Keys.RControlKey);
                //bool shiftPressed = !(IsKeyDown(Keys.LShiftKey) || IsKeyDown(Keys.RShiftKey) || (Keys)lParam.vkCode == Keys.LShiftKey || (Keys)lParam.vkCode == Keys.RShiftKey);

                this.KeyUp(this, new GameKeyboardHookEventArgs(lParam.vkCode, foregroundWindowHandle));
            }

            return CallNextHookEx(hookHandle, code, wParam, ref lParam);
        }

        public void AttachWindowHandle(IntPtr windowHandle)
        {
            if (!WindowHandles.Contains(windowHandle))
                WindowHandles.Add(windowHandle);
        }

        public void DetatchWindowHandle(IntPtr windowHandle)
        {
            if (WindowHandles.Contains(windowHandle))
                WindowHandles.Remove(windowHandle);

            if (WindowHandles.IsNullOrEmpty())
                Uninstall();
        }


        public void Install()
        {
            // make sure not already installed
            if (hookHandle != IntPtr.Zero)
                return;

            // need ssinstance handle to module to create a system-wide hook
            Module[] list = System.Reflection.Assembly.GetExecutingAssembly().GetModules();
            System.Diagnostics.Debug.Assert(list != null && list.Length > 0);

            // install system-wide hook
            hookHandle = SetWindowsHookEx(hookType,
                hookProc, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

            if (hookHandle != IntPtr.Zero)
                Installed = true;
        }

        public void Uninstall()
        {
            if (hookHandle != IntPtr.Zero)
            {
                // uninstall system-wide hook
                UnhookWindowsHookEx(hookHandle);
                hookHandle = IntPtr.Zero;
            }
        }

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        public static bool IsKeyDown(Keys key)
        {
            return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
        }

        public static bool IsKeyToggled(Keys key)
        {
            return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
        }
    }


    public class GameKeyboardHookEventArgs : EventArgs
    {

        public IntPtr WindowHandle;

        public Keys Key;
   
        public GameKeyboardHookEventArgs(UInt32 keyCode, IntPtr windowHandle)
        {
            this.Key = (Keys)keyCode;
            this.WindowHandle = windowHandle;
        }
    }
}
