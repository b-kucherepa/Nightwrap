namespace Nightwrap
{
    /// <summary>
    ///  The Form class which contains methods related to the settings GUI functioning.
    ///  Sends input to the main Program class and restores some initial values from it.
    /// </summary>
    public partial class GUIForm : Form
    {
        private const int MSECONDS_IN_SECOND = 1000; //used to convert NumUpDown seconds


        public GUIForm()
        {
            InitializeComponent();
            InitializeWindowIcon();
            InitializeEvents();
        }


        /// <summary>
        ///  Subscribes on interface elements interaction events
        /// </summary>
        private void InitializeEvents()
        {
            this.Shown += RecoverSettings;
            this.FormClosing += OnClosingForm;
            checkBoxEnable.CheckedChanged += OnEnableButtonClick;
            buttonClose.Click += Program.KillTheProcess;
            checkBoxStartup.CheckedChanged += OnStartupButtonClick;
            numericTimer.ValueChanged += OnNumericTimerValueChange;
        }


        /// <summary>
        /// Initializes icon for the window bar, and its tray icon
        /// </summary>
        private void InitializeWindowIcon()
        {
            Text = Program.NAME;
            Icon = new Icon(Program.ICON_NAME);
        }


        /// <summary>
        /// Sets GUI elements according to the application settings
        /// </summary>
        private void RecoverSettings(object? sender, EventArgs e)
        {
            if (Program.StartupIsEnabled)
            {
                checkBoxStartup.Checked = true;
                Hide();
            }
            else
            {
                checkBoxStartup.Checked = false;
            }

            if (Program.SaverIsEnabled)
            {
                checkBoxEnable.Checked = true;
            }
            else
            {
                checkBoxEnable.Checked = false;
            }

            numericTimer.Value = Program.PopupInterval / MSECONDS_IN_SECOND;
        }


        /// <summary>
        /// Sets GUI elements according to the application settings
        /// </summary>
        private void OnNumericTimerValueChange(object? sender, EventArgs e)
        {
            Program.PopupInterval = (int)(numericTimer.Value * MSECONDS_IN_SECOND);
        }


        /// <summary>
        /// Actions on screensaver popup enabling
        /// </summary>
        private void OnEnableButtonClick(object? sender, EventArgs e)
        {
            if (checkBoxEnable.Checked)
            {
                Program.SaverIsEnabled = true;
                checkBoxEnable.Text = "Stop";
            }
            else
            {
                Program.SaverIsEnabled = false;
                checkBoxEnable.Text = "Start";
            }
        }


        /// <summary>
        /// Actions on checking the "load on startup" checkbox
        /// </summary>
        private void OnStartupButtonClick(object? sender, EventArgs e)
        {
            if (checkBoxStartup.Checked)
            {
                Program.StartupIsEnabled = true;
            }
            else
            {
                Program.StartupIsEnabled = false;
            }
        }


        /// <summary>
        /// Blocks the classic WinForm red cross form closing the application completely
        /// and just hides the window instead
        /// </summary>
        private void OnClosingForm(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
