using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nightwrap
{
    public partial class ScreenSaverForm : Form
    {
        public ScreenSaverForm()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            KeyDown += new KeyEventHandler(OnSaverFormInput);
            MouseClick += new MouseEventHandler(OnSaverFormInput);
        }

        private void OnSaverFormInput(object? sender, EventArgs e)
        {
            Cursor.Show();
            Hide();
        }
    }
}
