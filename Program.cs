/*********************************************************************************************
 * 
*********************************************************************************************/

using System.Security.Policy;
using Timer = System.Windows.Forms.Timer;

namespace Nightwrap
{
    /// <summary>
    /// The main class of the application. It's both the entry point and a control point.
    /// </summary>
    /// <remarks>
    /// Such design was dictated by the small size of the app.Excluding the control functionality
    /// to a separate class would increase the sofrware complexity.
    /// And this software has nothing to be added... except some screensaver customization maybe?
    /// </remarks>
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

        private static bool _isEnabled;


        /// <summary>
        /// Shows if the main popup mechanics is enabled currently.
        /// </summary>
        /// <remarks>
        /// This property has dismantled "Enable" and "Disable" methods from before.
        /// That has simplified the access therefore lowering the complexity 
        /// but feels a bit wrong
        /// </remarks>
        internal static bool SaverIsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }


        /// <summary>
        /// Incapsulates methods returning the application startup on launch current state.
        /// </summary>
        /// <remarks>
        /// This property has dismantled "Enable" and "Disable" methods from before.
        /// That has simplified the access therefore lowering the complexity 
        /// but feels a bit wrong
        /// </remarks>
        internal static bool StartupIsEnabled
        { 
            get => Startup.CheckIfEnabled(); 
            set
            {
                if (value == true)
                {
                    Startup.Enable();
                }    
                else 
                { 
                    Startup.Disable();
                }
            }
        }


        /// <summary>
        /// A property which incapsulastes the timer popup interval.
        /// </summary>
        internal static int PopupInterval
        {
            get => _timer.Interval;
            set
            {
                if (value <= MINIMAL_INTERVAL)
                {
                    _timer.Interval = MINIMAL_INTERVAL;
                }
                else
                {
                    _timer.Interval = value;
                }
            }
        }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            //closes the new application instance if another instance was detected:
            if (CheckIfAlreadyLaunched())
                return;

            InitializeTrayIcon();
            InitializeTimer();
            InitializeInputInterception();
            InitializeForms();

            RestoreConfig();

            Application.Run(_guiForm);
        }


        /// <summary>
        /// Closes the application
        /// </summary>
        /// <remarks>
        /// (for real. Check the GUIForm "OnClosingForm" method)
        /// </remarks>
        internal static void KillTheProcess(object? sender, EventArgs e)
        {
            SaveConfig();
            _keyInterceptor.End();
            _mouseInterceptor.End();
            Application.Exit();
        }



        /// <summary>
        /// Resets popup timer
        /// </summary>
        internal static void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }


        /// <summary>
        /// Creates mutex to figure if it's the first instance of the application or not
        /// </summary>
        private static bool CheckIfAlreadyLaunched()
        {
            _mutex = new Mutex(true, NAME, out bool isCreatedNew);

            GC.KeepAlive(_mutex);

            return !isCreatedNew;
        }


        private static void InitializeForms()
        {
            _guiForm = new GUIForm();
            _saverForm = new ScreenSaverForm();
        }


        /// <summary>
        /// Initializes system tray icon
        /// </summary>
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


        /// <summary>
        /// Initializes popup timer
        /// </summary>
        private static void InitializeTimer()
        {
            _timer = new Timer();
            _timer.Tick += OnTimerTickPopSaver;
        }


        /// <summary>
        /// Initializes input interception to prevent the screensaver from appearing
        /// </summary>
        private static void InitializeInputInterception()
        {
            _mouseInterceptor = new MouseInterceptor();
            _keyInterceptor = new KeyboardInterceptor();
            _keyInterceptor.Begin();
            _mouseInterceptor.Begin();
        }


        /// <summary>
        /// Reads settings from a automatically generated application configuration file
        /// </summary>
        private static void RestoreConfig()
        {
            SaverIsEnabled = Properties.Settings.Default.isEnabled;
            PopupInterval = Properties.Settings.Default.interval;
        }


        /// <summary>
        /// Saves settings in a automatically generated application configuration file
        /// </summary>
        private static void SaveConfig()
        {
            Properties.Settings.Default.isEnabled = SaverIsEnabled;
            Properties.Settings.Default.interval = PopupInterval;
            Properties.Settings.Default.Save();
        }


        /// <summary>
        /// Shows GUI window
        /// </summary>
        private static void ShowGUI(object? sender, EventArgs e)
        {
            _guiForm.Show();
            Cursor.Show();
        }


        /// <summary>
        /// Popups the screensaver on a timer tick event
        /// </summary>
        private static void OnTimerTickPopSaver(object? sender, EventArgs e)
        {
            if (SaverIsEnabled)
            {
                _saverForm.Show();
                Cursor.Hide();
            }
        }
    }
}
