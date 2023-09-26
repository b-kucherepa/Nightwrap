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
        }

        private void InitializeEvents()
        {
            this.Shown += RecoverSettings;
            checkBoxEnable.CheckedChanged += OnEnableButtonClick;
            buttonClose.Click += Program.KillTheProcess;
            checkBoxStartup.CheckedChanged += OnStartupButtonClick;
            numericTimer.ValueChanged += OnNumericTimerValueChange;
        }

        private void InitializeIcon()
        {
            Text = Program.NAME;
            Icon = new Icon(Program.ICON_NAME);
        }

        private void RecoverSettings(object? sender, EventArgs e)
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

            numericTimer.Value = Program.PopupInterval / MSECONDS_IN_SECOND;
        }

        private void OnNumericTimerValueChange(object? sender, EventArgs e)
        {
            Program.PopupInterval = (int)(numericTimer.Value * MSECONDS_IN_SECOND);
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
