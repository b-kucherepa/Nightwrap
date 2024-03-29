﻿/* Based on Stephen Toub's code from June 2006 MSDN Magazine Online. Thank you, Stephen!*/


using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Nightwrap
{
    /// <summary>
    /// Hooks Windows keyboard input to prevent the screensaver popup 
    /// even then application is working in the background mode.
    /// It resets the timer upon detecting any key input.
    /// </summary>
    class KeyboardInterceptor
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        internal void Begin()
        {
            _hookID = SetHook(_proc);
        }


        internal void End()
        {
            UnhookWindowsHookEx(_hookID);
        }


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule? curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private delegate IntPtr LowLevelKeyboardProc(int nCode,
            IntPtr wParam, IntPtr lParam);


        /// <summary>
        /// Registers the input and calls the popup timer reset method
        /// </summary>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                Program.ResetTimer();
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                Program.ResetTimer();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        /*Attaching Windows dlls required:*/

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr CallNextHookEx(IntPtr hhk,
            int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}