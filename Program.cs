/*********************************************************************************************
 * The main class of the application. It's both the entry point and a control point.
 * Such design was dictated by the small size of the app. Excluding the control functionality
 * to a separate class would increase the sofrware complexity. 
 * And this software has nothing to be added... except some screensaver customization maybe?
*********************************************************************************************/

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

        private static bool _isEnabled;


        /* A property which incapsulates if the main popup mechanics is enabled currently.
         * This property has dismantled "Enable" and "Disable" methods from before.
         * That has simplified the access therefore lowering the complexity 
         * but feels a bit wrong: */
        internal static bool SaverIsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }


        /* Same is true for this property covering the program startup on launch state.
         * Also it feels too complicated for a thing as simple as property: */
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


        /* A property which incapsulastes the timer popup interval.
         * A bit complicated thing too: */
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

            //Closes the new application instance if another instance was detected:
            if (CheckIfAlreadyLaunched())
                return;

            InitializeTrayIcon();
            InitializeTimer();
            InitializeInputInterception();
            InitializeForms();

            RestoreConfig();

            Application.Run(_guiForm);
        }

        /* Close the application (for real. Check the GUIForm "OnClosingForm" method): */
        internal static void KillTheProcess(object? sender, EventArgs e)
        {
            SaveConfig();
            _keyInterceptor.End();
            _mouseInterceptor.End();
            Application.Exit();
        }


        /* Resets popup timer: */
        internal static void ResetTimer()
        {
            _timer.Stop();
            _timer.Start();
        }

        /* Creates mutex to figure if it's the first instance of the application or not: */
        private static bool CheckIfAlreadyLaunched()
        {
            _mutex = new Mutex(true, NAME, out bool isCreatedNew);

            GC.KeepAlive(_mutex);

            return !isCreatedNew;
        }


        /* Initializes forms: */
        private static void InitializeForms()
        {
            _guiForm = new GUIForm();
            _saverForm = new ScreenSaverForm();
        }


        /* Initializes system tray icon: */
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


        /* Initializes popup timer: */
        private static void InitializeTimer()
        {
            _timer = new Timer();
            _timer.Tick += OnTimerTickPopSaver;
        }


        /* Initializes input interception to prevent the screensaver from appearing: */
        private static void InitializeInputInterception()
        {
            _mouseInterceptor = new MouseInterceptor();
            _keyInterceptor = new KeyboardInterceptor();
            _keyInterceptor.Begin();
            _mouseInterceptor.Begin();
        }


        /* Reads settings from a automatically generated application configuration file: */
        private static void RestoreConfig()
        {
            SaverIsEnabled = Properties.Settings.Default.isEnabled;
            PopupInterval = Properties.Settings.Default.interval;
        }


        /* Saves settings in a automatically generated application configuration file: */
        private static void SaveConfig()
        {
            Properties.Settings.Default.isEnabled = SaverIsEnabled;
            Properties.Settings.Default.interval = PopupInterval;
            Properties.Settings.Default.Save();
        }


        /* Shows GUI window: */
        private static void ShowGUI(object? sender, EventArgs e)
        {
            _guiForm.Show();
            Cursor.Show();
        }


        /* Popups the screensaver on a timer tick event: */
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
