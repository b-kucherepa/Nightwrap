using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Nightwrap
{
    internal static class Program
    {
        public const string NAME = "Nightwrap";
        public const string ICON_NAME = "Black-dress.ico";

        internal static bool isEnabled = false;

        static readonly Form guiForm = new GUIForm();
        static readonly Form saverForm = new ScreenSaverForm();

        static readonly SaverTimer saverTimer = new();

        static NotifyIcon trayIcon = new();

        static InterceptMouse interceptMouse = new();
        static InterceptKeys interceptKeys = new();


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            InitializeTrayIcon();
            saverTimer.Tick += new EventHandler(OnTimerTick);
            interceptKeys.Begin();
            interceptMouse.Begin();
            Application.Run(guiForm);
        }

        private static void InitializeTrayIcon()
        {
            trayIcon.Icon = new Icon(ICON_NAME);
            trayIcon.Text = NAME;
            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            trayIcon.ContextMenuStrip.Items.Add("Open", null, ShowGUI);
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, KillTheProcess);
            trayIcon.Visible = true;
            trayIcon.Click += new EventHandler(ShowGUI);
        } 

        public static void EnableSaver()
        {
            isEnabled = true;
        }

        public static void DisableSaver()
        {
            isEnabled = false;
        }

        internal static void PopSaver()
        {
            if (isEnabled)
            {
                saverForm.Show();
                Cursor.Hide();
            }
        }

        static private void OnTimerTick(object? sender, EventArgs e)
        {
            PopSaver();
        }

        static void ShowGUI(object? sender, EventArgs e)
        {
            guiForm.Show();
            Cursor.Show();
        }
        public static void KillTheProcess()
        {
            interceptKeys.End();
            interceptMouse.End();
            Application.Exit();
        }

        private static void KillTheProcess(object? sender, EventArgs e)

        {
            interceptKeys.End();
            interceptMouse.End();
            Application.Exit();
        }

        public static void SetTimer(int timer) => saverTimer.Set(timer);
        public static void ResetTimer() => saverTimer.Reset();
        public static void EnableStartup()=>RegistryManager.EnableStartup();
        public static void DisableStartup()=>RegistryManager.DisableStartup();
        public static bool CheckIfInStartup()=>RegistryManager.CheckIfInStartup();
    }
}
