using System.Configuration;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Nightwrap
{
    internal static class Program
    {
        public const string NAME = "Nightwrap";
        public const string ICON_NAME = "Black-dress.ico";
        public static readonly string APPLICATION_FOLDER_PATH = Environment.CurrentDirectory;
        public static readonly string APPLICATION_EXE_PATH = Environment.ProcessPath;

        public static int PopupInterval
        {
            get => int.Parse(ConfigurationManager.AppSettings["interval"]);
            set
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (ConfigurationManager.AppSettings["interval"] is null)
                    ConfigurationManager.AppSettings.Add("interval", value.ToString());
                else
                    ConfigurationManager.AppSettings["interval"] = value.ToString();

                configFile.Save(ConfigurationSaveMode.Modified);
            }
        }

        private static Mutex _mutex;

        private static bool _isEnabled = false;

        private static NotifyIcon _trayIcon = new();

        private static readonly Timer _timer = new();

        private static MouseInterceptor _mouseInterceptor = new();
        private static KeyboardInterceptor _keyInterceptor = new();

        private static readonly Form _guiForm = new GUIForm();
        private static readonly Form _saverForm = new ScreenSaverForm();


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            if (CheckIfAlreadyLaunched())
                return;

            InitializeTrayIcon();
            InitializeTimer();
            InitializeInputInterception();

            Application.Run(_guiForm);
        }

        private static bool CheckIfAlreadyLaunched()
        {
            bool isCreatedNew;
            _mutex = new Mutex(true, NAME, out isCreatedNew);

            GC.KeepAlive(_mutex);

            return !isCreatedNew;
        }

        private static void InitializeTrayIcon()
        {
            _trayIcon.Icon = new Icon(ICON_NAME);
            _trayIcon.Text = NAME;
            _trayIcon.ContextMenuStrip = new ContextMenuStrip();
            _trayIcon.ContextMenuStrip.Items.Add("Open", null, ShowGUI);
            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, KillTheProcess);
            _trayIcon.Visible = true;
            _trayIcon.Click += new EventHandler(ShowGUI);
        }

        private static void InitializeTimer()
        {
            _timer.Tick += new EventHandler(OnTimerTick);
            _timer.Interval = PopupInterval;
        }

        private static void InitializeInputInterception()
        {
            _keyInterceptor.Begin();
            _mouseInterceptor.Begin();
        }
        public static void EnableSaver()
        {
            _isEnabled = true;
        }

        public static void DisableSaver()
        {
            _isEnabled = false;
        }

        internal static void PopSaver()
        {
            if (_isEnabled)
            {
                _saverForm.Show();
                Cursor.Hide();
            }
        }

        static private void OnTimerTick(object? sender, EventArgs e)
        {
            PopSaver();
        }

        static void ShowGUI(object? sender, EventArgs e)
        {
            _guiForm.Show();
            Cursor.Show();
        }
        public static void KillTheProcess()
        {
            _keyInterceptor.End();
            _mouseInterceptor.End();
            Application.Exit();
        }

        private static void KillTheProcess(object? sender, EventArgs e)
        {
            _keyInterceptor.End();
            _mouseInterceptor.End();
            Application.Exit();
        }

        public static void SetTimer(int interval)
        {
            _timer.Interval = interval;
            PopupInterval = interval;
        }
        public static void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }
        public static void EnableStartup() => Startup.Enable();
        public static void DisableStartup() => Startup.Disable();
        public static bool StartupIsEnabled => Startup.CheckIfEnabled();
    }
}
