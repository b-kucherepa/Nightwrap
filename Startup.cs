﻿using IWshRuntimeLibrary;
using Microsoft.Win32;
using static System.Environment;

namespace Nightwrap
{
    static internal class Startup
    {
        const string REGISTRY_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string STARTUP_PATH = GetFolderPath(SpecialFolder.Startup);
        private static readonly string SHORTCUT_EXTENSION = ".lnk";

        public static bool CheckIfEnabled()
        {
            return Registry.GetValue(Registry.CurrentUser.Name + "\\" + REGISTRY_PATH, Program.NAME, null) is not null;
        }

        internal static void Enable()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.SetValue(Program.NAME, "\"" + Application.ExecutablePath + "\"");
            }
            CreateShortcut();
        }

        public static void Disable()
        {
            RemoveShortcut();
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                key.DeleteValue(Program.NAME, false);
            }
        }

        private static void CreateShortcut()
        {
            WshShell wsh = new();
            IWshShortcut shortcut = wsh.CreateShortcut(STARTUP_PATH + "\\" + Program.NAME + SHORTCUT_EXTENSION) as IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = Program.APPLICATION_EXE_PATH;
            shortcut.WindowStyle = 1;
            shortcut.Description = "";
            shortcut.WorkingDirectory = Program.APPLICATION_FOLDER_PATH;
            shortcut.IconLocation = Program.APPLICATION_FOLDER_PATH + "\\" + Program.ICON_NAME;
            shortcut.Save();
        }

        private static void RemoveShortcut()
        {
            try
            {
                System.IO.File.Delete(STARTUP_PATH + "\\" + Program.NAME + SHORTCUT_EXTENSION);
            }
            catch
            {
                //in that case still continuing the program
            }
        }
    }
}