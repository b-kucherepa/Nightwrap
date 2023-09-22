using Microsoft.Win32;

namespace Nightwrap
{
    static internal class RegistryManager
    {
        const string REGISTRY_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        static internal void EnableStartup()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.SetValue(Program.NAME, "\"" + Application.ExecutablePath + "\"");
            }
        }
        static public void DisableStartup()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.DeleteValue(Program.NAME, false);
            }
        }

        static public bool CheckIfInStartup()
        {
            return (Registry.GetValue(Registry.CurrentUser.Name + "\\" + REGISTRY_PATH, Program.NAME, null) != null);
        }
    }
}
