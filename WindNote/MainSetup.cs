using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindNote.Assemblies;
using WindNote.Build.Setup;

namespace WindNote
{
    class MainSetup
    {
        private static string AppDomainName = "AppDomainPortableSetup";

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)] 
        static void Main()
        {
            MainSetup.InitDataDirectory();

            string appDomainName = AppDomain.CurrentDomain.FriendlyName;
            if (appDomainName != MainSetup.AppDomainName)
            {
                MainSetup.LaunchAppPortableSetup();
            }
            else
            {
                MainSetup.LoadUnmanagedDlls();
                MainSetup.Run();
            }
        }

        private static void InitDataDirectory()
        {
            string dataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\WindNote";
            MainSetup.InitDirectory("DataDirectory", dataDirectory);
        }

        private static void InitDirectory(string directoryKey, string directoryPath)
        {
            MainSetup.CreateDirectoryIfDoesNotExist(directoryPath);
            MainSetup.AddDirectoryToAppDomain(directoryKey, directoryPath);
        }

        private static void CreateDirectoryIfDoesNotExist(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        private static void AddDirectoryToAppDomain(string directoryKey, string directoryPath)
        {
            AppDomain.CurrentDomain.SetData(directoryKey, directoryPath);
        }

        static void LaunchAppPortableSetup()
        {
            IAppLauncherFactory factory = new AppLauncherFactory();
            AppLauncher launcher = factory.CreateAppLauncher();
            MainSetup.LaunchAppPortableSetup(launcher);
        }

        static void LaunchAppPortableSetup(AppLauncher launcher)
        {
            launcher.LoadConfigFromEmbeddedResources("WindNote.WindNote.exe.config", "WindNote.exe.config");
            launcher.Start("WindNote", MainSetup.AppDomainName);
        }

        static void LoadUnmanagedDlls()
        {
            IDllLoaderFactory factory = new DllLoaderFactory();
            UnmanagedDllLoader dllLoader = factory.CreateUnmanagedDllLoader();
            MainSetup.LoadUnmanagedDllsPlatform(dllLoader);
        }

        static void LoadUnmanagedDllsPlatform(UnmanagedDllLoader dllLoader)
        {
            if (System.Environment.Is64BitProcess == true)
            {
                MainSetup.LoadUnmanagedDlls64bit(dllLoader);
            }
            else
            {
                MainSetup.LoadUnmanagedDlls32bit(dllLoader);
            }
        }

        static void LoadUnmanagedDlls64bit(UnmanagedDllLoader dllLoader)
        {
            dllLoader.LoadFromEmbeddedResources("WindNote.Lib.x64.SQLite.Interop.dll", "SQLite.Interop.dll");
        }

        static void LoadUnmanagedDlls32bit(UnmanagedDllLoader dllLoader)
        {
            dllLoader.LoadFromEmbeddedResources("WindNote.Lib.x86.SQLite.Interop.dll", "SQLite.Interop.dll");
        }

        static void Run()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run(); 
        }

    }
}
