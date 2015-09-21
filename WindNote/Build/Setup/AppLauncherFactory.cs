using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindNote.Build.EmbeddedResources;

namespace WindNote.Build.Setup
{
    public interface IAppLauncherFactory
    {
        AppLauncher CreateAppLauncher();
    }

    public class AppLauncherFactory : IAppLauncherFactory
    {
        public AppLauncher CreateAppLauncher()
        {
            EmbeddedFileExtractor extractor = new EmbeddedFileExtractor();
            AppDomainSetup setup = new AppDomainSetup();
            return new AppLauncher(extractor, setup);
        }
    }

}
