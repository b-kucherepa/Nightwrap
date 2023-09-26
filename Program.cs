using System.Configuration;
using Timer = System.Windows.Forms.Timer;

namespace Nightwrap
{
    internal static class Program
    {
        internal const string NAME = "Nightwrap";
        internal const string ICON_NAME = "Black-dress.ico";
        internal static readonly string APPLICATION_FOLDER_PATH = Environment.CurrentDirectory;
        internal static readonly string APPLICATION_EXE_PATH = Environment.ProcessPath;

        private const int MINIMAL_INTERVAL = 5000;

        private static NotifyIcon _trayIcon;

        private static Timer _timer;

        private static MouseInterceptor _mouseInterceptor;
        private static KeyboardInterceptor _keyInterceptor;

        private static Form _guiForm;
        private static Form _saverForm;

        private static Mutex _mutex;

        private static bool _isEnabled = false;

        internal static int PopupInterval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                if (value <= MINIMAL_INTERVAL)
                    _timer.Interval = MINIMAL_INTERVAL;
                else
                    _timer.Interval = value;
            }
        }

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
            InitializeForms();

            RestoreConfig();

            Application.Run(_guiForm);
        }

        private static void InitializeForms()
        {
            _guiForm = new GUIForm();
            _saverForm = new ScreenSaverForm();
        }

        private static bool CheckIfAlreadyLaunched()
        {
            _mutex = new Mutex(true, NAME, out bool isCreatedNew);

            GC.KeepAlive(_mutex);

            return !isCreatedNew;
        }

        private static void InitializeTrayIcon()
        {
            _trayIcon = new NotifyIcon();
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
            _timer = new Timer();
            _timer.Tick += new EventHandler(OnTimerTick);
        }

        private static void InitializeInputInterception()
        {
            _mouseInterceptor = new MouseInterceptor();
            _keyInterceptor = new KeyboardInterceptor();
            _keyInterceptor.Begin();
            _mouseInterceptor.Begin();
        }
        internal static void EnableSaver()
        {
            _isEnabled = true;
        }

        internal static void DisableSaver()
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

        internal static void KillTheProcess(object? sender, EventArgs e)
        {
            SaveConfig();
            _keyInterceptor.End();
            _mouseInterceptor.End();
            Application.Exit();
        }

        internal static void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }

        internal static void EnableStartup() => Startup.Enable();

        internal static void DisableStartup() => Startup.Disable();

        internal static bool StartupIsEnabled => Startup.CheckIfEnabled();

        private static void RestoreConfig()
        {
            PopupInterval = Properties.Settings.Default.interval;
        }

        private static void SaveConfig()
        {
            Properties.Settings.Default.interval = PopupInterval;
            Properties.Settings.Default.Save();
        }
    }
}
