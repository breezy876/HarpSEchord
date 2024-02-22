using Common.Helpers;
using Common.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Common.Interop
{
    public class KeyboardInputSimulator
    {


        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;
        private const int WM_CHAR = 0x0102;
        private const int MAPVK_VK_TO_CHAR = 2;

        struct INPUT
        {
            public int type;
            public InputUnion un;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint GetLastError();

        Dictionary<int, char> MappedCharacters { get; set; }

        public KeyboardInputSimulator()
        {
            MappedCharacters = new Dictionary<int, char>();
        }

        public IntPtr WindowHandle { get; set; }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public void SendKeyPress(object p)
        {
            throw new NotImplementedException();
        }

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
     

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern bool SetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        public void PostKeyDown(int key)
        {

            PostMessage(WindowHandle, WM_KEYDOWN, (IntPtr)key, (IntPtr)0);
        }

        public void PostKeyUp(int key)
        {
            PostMessage(WindowHandle, WM_KEYUP, (IntPtr)key, (IntPtr)0);
        }


        public async Task PostKeyDown(Keys key)
        {
            var baseKey = key & ~Keys.Control & key & ~Keys.Shift & key & ~Keys.Alt;

            if (baseKey != key)
            {
                for (var i = 0; i < 5; i++)
                {
                    if ((key & Keys.Control) == Keys.Control) SendMessage(WindowHandle, WM_KEYDOWN, (IntPtr)Keys.ControlKey, (IntPtr)0);
                    if ((key & Keys.Alt) == Keys.Alt) SendMessage(WindowHandle, WM_SYSKEYDOWN, (IntPtr)Keys.Menu, (IntPtr)0);
                    if ((key & Keys.Shift) == Keys.Shift) SendMessage(WindowHandle, WM_KEYDOWN, (IntPtr)Keys.ShiftKey, (IntPtr)0);
                    await Task.Delay(5);
                }
            }

            PostMessage(WindowHandle, WM_KEYDOWN, (IntPtr)key, (IntPtr)0);
        }

        public async Task PostKeyUp(Keys key)
        {
            var baseKey = key & ~Keys.Control & key & ~Keys.Shift & key & ~Keys.Alt;

            if (baseKey != key)
            {
                if ((key & Keys.Shift) == Keys.Shift)
                {
                    await Task.Delay(5);
                    PostMessage(WindowHandle, WM_KEYUP, (IntPtr)Keys.ShiftKey, (IntPtr)0);
                }

                if ((key & Keys.Alt) == Keys.Alt)
                {
                    await Task.Delay(5);
                    PostMessage(WindowHandle, WM_SYSKEYUP, (IntPtr)Keys.Menu, (IntPtr)0);
                }

                if ((key & Keys.Control) == Keys.Control)
                {
                    await Task.Delay(5);
                    PostMessage(WindowHandle, WM_KEYUP, (IntPtr)Keys.ControlKey, (IntPtr)0);
                }
            }

            PostMessage(WindowHandle, WM_KEYUP, (IntPtr)key, (IntPtr)0);
        }

        public void PostCharacter(char ch)
        {
            PostMessage(WindowHandle, WM_CHAR, (IntPtr)(uint)ch, (IntPtr)0);
        }

        public void SendText(string text)
        {
            foreach(char ch in text)
            {
                PostCharacter(ch);
            }
        }
        public async Task PostKeyPress(Keys key)
        {
            await PostKeyDown(key);



            await PostKeyUp(key);

        }

        public async Task SendKeyPress(Keys key)
        {
            await SendKeyDown(key);



            await SendKeyUp(key);

        }

        public async Task SendKeyDown(Keys key)
        {
            //uint repeatCount = 0;
            //uint scanCode = MapVirtualKey((uint)key, 0);
            //uint extended = 0;
            //uint context = 0;
            //uint previousState = 0;
            //uint transition = 0;
            //uint lParamDown = repeatCount
            //    | (scanCode << 16)
            //    | (extended << 24)
            //    | (context << 29)
            //    | (previousState << 30)
            //    | (transition << 31);

            var baseKey = key & ~Keys.Control & key & ~Keys.Shift & key & ~Keys.Alt;

            if (baseKey != key)
            {
                for (var i = 0; i < 5; i++)
                {
                    if ((key & Keys.Control) == Keys.Control) SendMessage(WindowHandle, WM_KEYDOWN, (IntPtr)Keys.ControlKey, (IntPtr)0);
                    if ((key & Keys.Alt) == Keys.Alt) SendMessage(WindowHandle, WM_SYSKEYDOWN, (IntPtr)Keys.Menu, (IntPtr)0);
                    if ((key & Keys.Shift) == Keys.Shift) SendMessage(WindowHandle, WM_KEYDOWN, (IntPtr)Keys.ShiftKey, (IntPtr)0);
                    await Task.Delay(5);
                }
            }

            SendMessage(WindowHandle, WM_KEYDOWN, (IntPtr)baseKey, (IntPtr)0);

        }

        public async Task SendKeyUp(Keys key)
        {
            //uint repeatCount = 0;
            //uint scanCode = MapVirtualKey((uint)key, 0);
            //uint extended = 0;
            //uint context = 0;
            //uint previousState = 1;
            //uint transition = 1;
            //uint lParamUp = repeatCount
            //    | (scanCode << 16)
            //    | (extended << 24)
            //    | (context << 29)
            //    | (previousState << 30)
            //    | (transition << 31);

            SendMessage(WindowHandle, WM_KEYUP, (IntPtr)key, (IntPtr)0);


            var baseKey = key & ~Keys.Control & key & ~Keys.Shift & key & ~Keys.Alt;


            if (baseKey != key)
            {
                if ((key & Keys.Shift) == Keys.Shift)
                {
                    await Task.Delay(5);
                    SendMessage(WindowHandle, WM_KEYUP, (IntPtr)Keys.ShiftKey, (IntPtr)0);
                }

                if ((key & Keys.Alt) == Keys.Alt)
                {
                    await Task.Delay(5);
                   SendMessage(WindowHandle, WM_SYSKEYUP, (IntPtr)Keys.Menu, (IntPtr)0);
                }

                if ((key & Keys.Control) == Keys.Control)
                {
                    await Task.Delay(5);
                    SendMessage(WindowHandle, WM_KEYUP, (IntPtr)Keys.ControlKey, (IntPtr)0);
                }
            }

        }

        public void SendKeyInput(int key)
        { 

            List<INPUT> keyList = new List<INPUT>();

            INPUT keyDown = new INPUT
            {
                type = 1,
                un = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = (ushort)key,
                        dwFlags = 0x0004,
                    }
                }
            };
            keyList.Add(keyDown);

            INPUT keyUp = new INPUT
            {
                type = 1,
                un = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = 0,
                        wScan = (ushort)key,
                        dwFlags = 0x0004 | 0x0002,
                    }
                }
            };
            keyList.Add(keyUp);

            SendInput((uint)keyList.Count, keyList.ToArray(), Marshal.SizeOf(typeof(INPUT)));
        }



        //public static void PressKey(char ch, bool press)
        //{
        //    byte vk = VkKeyScan(ch);
        //    PressKey(vk, press);
        //}

        //public static void PressKey(int vk, bool press)
        //{
        //    ushort scanCode = (ushort)MapVirtualKey((ushort)vk, 0);

        //    //Console.WriteLine("SendInput:: VK: " + (ushort)vk + " (" + vk + ") <-> SC: " + (ushort)(scanCode & 0xff));

        //    if (press)
        //        KeyDown(scanCode);
        //    else
        //        KeyUp(scanCode);
        //}

        //public static void KeyDown(ushort scanCode)
        //{
        //    //Console.WriteLine("Key Down (SC): " + (ushort)(scanCode & 0xff));
        //    INPUT[] inputs = new INPUT[1];

        //    inputs[0].type = INPUT_KEYBOARD;
        //    inputs[0].ki.wScan = (ushort)(scanCode & 0xff);
        //    inputs[0].ki.dwFlags = KEYEVENTF_SCANCODE;
        //    inputs[0].ki.time = 0;
        //    inputs[0].ki.dwExtraInfo = IntPtr.Zero;

        //    uint intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
        //    if (intReturn != 1)
        //    {
        //        throw new Exception("Could not send key: " + scanCode);
        //    }
        //}

    }
}
