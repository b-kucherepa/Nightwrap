using System.ComponentModel;

namespace Nightwrap
{

    /// <summary>
    /// This Form contains code for "waking up" (removing the black popup screensaver)
    /// </summary>
    public partial class ScreenSaverForm : Form
    {
        public ScreenSaverForm()
        {
            InitializeComponent();
            InitializeEvents();
        }


        /// <summary>
        /// Subscribes on input events which are "waking it up"
        /// </summary>
        private void InitializeEvents()
        {
            KeyDown += new KeyEventHandler(OnSaverFormInput);
            MouseClick += new MouseEventHandler(OnSaverFormInput);
        }


        /// <summary>
        /// Hides the form itself on any input
        /// </summary>
        private void OnSaverFormInput(object? sender, EventArgs e)
        {
            Cursor.Show();
            Hide();
        }
    }
}
