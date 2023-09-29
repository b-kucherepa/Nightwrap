/*************************************************************************************************
 * This is the manually created counterpart of the designer-generated class part.
 * This part contains methods related to the settings GUI functioning.
 * The part sends input to the main Program class and restores some initial values from it.
*************************************************************************************************/

using System.ComponentModel;


namespace Nightwrap
{
    public partial class GUIForm : Form
    {
        private const int MSECONDS_IN_SECOND = 1000; //used to convert NumUpDown seconds


        public GUIForm()
        {
            InitializeComponent();
            InitializeWindowIcon();
            InitializeEvents();
        }


        /*Subscribes on interface elements interaction events:*/
        private void InitializeEvents()
        {
            this.Shown += RecoverSettings;
            checkBoxEnable.CheckedChanged += OnEnableButtonClick;
            buttonClose.Click += Program.KillTheProcess;
            checkBoxStartup.CheckedChanged += OnStartupButtonClick;
            numericTimer.ValueChanged += OnNumericTimerValueChange;
        }


        /*Initializes icon for the window bar, and its tray icon:*/
        private void InitializeWindowIcon()
        {
            Text = Program.NAME;
            Icon = new Icon(Program.ICON_NAME);
        }


        /*Sets GUI elements according to the application settings:*/
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
                Program.StartupIsEnabled = true;
            }
            else
            {
                checkBoxEnable.Checked = false;
            }

            numericTimer.Value = Program.PopupInterval / MSECONDS_IN_SECOND;
        }


        /*Sets GUI elements according to the application settings:*/

        private void OnNumericTimerValueChange(object? sender, EventArgs e)
        {
            Program.PopupInterval = (int)(numericTimer.Value * MSECONDS_IN_SECOND);
        }


        /*Actions on screensaver popup enabling:*/

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


        /*Actions on checking the "load on startup" checkbox:*/

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


        /*Blocks the classic WinForm red cross form closing the application completely
         * and just hides the window instead*/
        private void OnClosingForm(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
