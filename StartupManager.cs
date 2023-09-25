using Microsoft.Win32;
using static System.Environment;
using Shell32;
using IWshRuntimeLibrary;

namespace Nightwrap
{
    static internal class RegistryManager
    {
        const string REGISTRY_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string STARTUP_PATH = GetFolderPath(SpecialFolder.Startup);
        private static readonly string SHORTCUT_NAME = Program.NAME + ".lnk";
        static internal void EnableStartup()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.SetValue(Program.NAME, "\"" + Application.ExecutablePath + "\"");
            }
            CreateShortcut();
        }

        static private void CreateShortcut()
        {
            WshShell wsh = new ();
            IWshShortcut shortcut = wsh.CreateShortcut(STARTUP_PATH+"\\"+SHORTCUT_NAME) as IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = Program.APPLICATION_EXE_PATH;
            shortcut.WindowStyle = 1;
            shortcut.Description = "";
            shortcut.WorkingDirectory = Program.APPLICATION_FOLDER_PATH;
            shortcut.IconLocation = Program.APPLICATION_FOLDER_PATH + "\\" + Program.ICON_NAME;
            shortcut.Save();
        }
        static public void DisableStartup()
        {
            RemoveShortcut();
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.DeleteValue(Program.NAME, false);
            }
        }

        private static void RemoveShortcut()
        {
            try 
            {
                System.IO.File.Delete(STARTUP_PATH + "\\" + SHORTCUT_NAME); 
            }
            catch
            {
                //in that case still continuing the program
            }
        }

        static public bool CheckIfInStartup()
        {
            return (Registry.GetValue(Registry.CurrentUser.Name + "\\" + REGISTRY_PATH, Program.NAME, null) != null);
        }
    }
}
