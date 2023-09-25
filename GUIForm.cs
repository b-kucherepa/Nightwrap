using System.ComponentModel;

namespace Nightwrap
{
    public partial class GUIForm : Form
    {
        private const int MSECONDS_IN_SECOND = 1000;
        public GUIForm()
        {
            InitializeComponent();
            InitializeIcon();
            InitializeEvents();
            RecoverSettings();
        }

        private void InitializeEvents()
        {
            checkBoxEnable.CheckedChanged += OnEnableButtonClick;
            buttonClose.Click += OnCloseButtonClick;
            checkBoxStartup.CheckedChanged += OnStartupButtonClick;
            numericTimer.ValueChanged += OnNumericTimerValueChange;
        }

        private void InitializeIcon()
        {
            Text = Program.NAME;
            Icon = new Icon(Program.ICON_NAME);
        }

        private void RecoverSettings()
        {
            if (Program.StartupIsEnabled)
            {
                checkBoxStartup.Checked = true;
                checkBoxEnable.Checked = true;
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                checkBoxStartup.Checked = false;
            }

            numericTimer.Value = Program.PopupInterval/MSECONDS_IN_SECOND;
        }

        private void OnNumericTimerValueChange(object? sender, EventArgs e)
        {
            Program.SetTimer((int)numericTimer.Value*MSECONDS_IN_SECOND);
        }

        private void OnCloseButtonClick(object? sender, EventArgs e)
        {
            Program.KillTheProcess();
        }

        private void OnEnableButtonClick(object? sender, EventArgs e)
        {
            if (checkBoxEnable.Checked)
            {
                Program.EnableSaver();
                checkBoxEnable.Text = "Stop";
            }
            else
            {
                Program.DisableSaver();
                checkBoxEnable.Text = "Start";
            }
        }

        private void OnStartupButtonClick(object? sender, EventArgs e)
        {
            if (checkBoxStartup.Checked)
            {
                Program.EnableStartup();
            }
            else
            {
                Program.DisableStartup();
            }
        }

        private void OnClosingForm(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
