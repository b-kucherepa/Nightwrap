using System.ComponentModel;

namespace Nightwrap
{
    public partial class GUIForm : Form
    {
        public GUIForm()
        {
            InitializeComponent();
            InitializeIcon();
            InitializeEvents();
            checkBoxStartup.Checked = Program.CheckIfInStartup();
            
        }

        private void InitializeEvents()
        {
            checkBoxEnable.CheckedChanged += OnEnableButtonClick;
            buttonClose.Click += OnCloseButtonClick;
            checkBoxStartup.CheckedChanged += OnStartupButtonClick;
            numericTimer.ValueChanged += OnGUITimerValueChange;
        }

        private void InitializeIcon()
        {
            Text = Program.NAME;
            Icon = new Icon(Program.ICON_NAME);
        }

            private void OnGUITimerValueChange(object? sender, EventArgs e)
        {
            Program.SetTimer((int)numericTimer.Value);
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
