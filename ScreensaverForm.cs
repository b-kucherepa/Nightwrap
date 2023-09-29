/*************************************************************************************************
 * This is the manually created counterpart of the designer-generated class part.
 * This part contains code for "waking up" (removing the black popup screensaver)
*************************************************************************************************/

namespace Nightwrap
{
    public partial class ScreenSaverForm : Form
    {
        public ScreenSaverForm()
        {
            InitializeComponent();
            InitializeEvents();
        }


        /*Subscribes on input events which are "waking it up":*/
        private void InitializeEvents()
        {
            KeyDown += new KeyEventHandler(OnSaverFormInput);
            MouseClick += new MouseEventHandler(OnSaverFormInput);
        }


        /*Hides itself on any input*/
        private void OnSaverFormInput(object? sender, EventArgs e)
        {
            Cursor.Show();
            Hide();
        }
    }
}
