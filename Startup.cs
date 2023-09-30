/*************************************************************************************************

*************************************************************************************************/

using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Environment;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Nightwrap
{
    /// <summary>
    /// Manages the application startup on Windows system loading
    /// and contains such methods as including regisry operation or
    /// creating the startup shortcut file in the Windows Start up folder.
    /// </summary>
    static internal class Startup
    {
        private const string REGISTRY_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string STARTUP_PATH = GetFolderPath(SpecialFolder.Startup);
        private static readonly string SHORTCUT_EXTENSION = ".lnk";
        private static readonly string SHORTCUT_PATH = STARTUP_PATH
            + "\\" + Program.NAME + SHORTCUT_EXTENSION;


        /// <summary>
        /// Checks if the application added to the Windows Startup by checking the registry
        /// </summary>
        /// <returns>
        /// Bool value if enabled or not
        /// </returns>
        internal static bool CheckIfEnabled()
        {
            string registryValuePath = Registry.CurrentUser.Name + "\\" + REGISTRY_PATH;
            return Registry.GetValue(registryValuePath, Program.NAME, null) is not null;
        }


        /// <summary>
        /// Enables the application loading on startup.
        /// </summary>
        /// <remarks>
        /// Adds a key into register for working properly on older Windows systems
        /// </remarks>
        internal static void Enable()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.SetValue(Program.NAME, "\"" + Application.ExecutablePath + "\"");
            }

            CreateShortcut();
        }


        /// <summary>
        /// Disables the application loading on startup
        /// </summary>
        public static void Disable()
        {
            RemoveShortcut();
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.DeleteValue(Program.NAME, false);
            }
        }


        /// <summary>
        /// Creates the startup shortcut file in the Windows Start up folder.
        /// </summary>
        /// <remarks>
        /// It's essential for proper startup functioning on Windows 11
        /// </remarks>
        private static void CreateShortcut()
        {
            WshShell wsh = new();

            IWshShortcut shortcut = wsh.CreateShortcut(SHORTCUT_PATH) as IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = Program.APPLICATION_EXE_PATH;
            shortcut.WindowStyle = 1;
            shortcut.Description = "";
            shortcut.WorkingDirectory = Program.APPLICATION_FOLDER_PATH;
            shortcut.IconLocation = Program.APPLICATION_FOLDER_PATH + "\\"
                + Program.ICON_NAME;
            shortcut.Save();
        }


        /// <summary>
        /// Removes the shortcut
        /// </summary>
        private static void RemoveShortcut()
        {
            try
            {
                System.IO.File.Delete(SHORTCUT_PATH);
            }
            catch
            {
                //in that case do nothing and just continuing the program
            }
        }
    }
}
