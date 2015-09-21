using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindNote.Build.EmbeddedResources;

namespace WindNote.Build.Setup
{
    public class AppLauncher
    {
        private EmbeddedFileExtractor Extractor;
        private AppDomainSetup Setup;

        public AppLauncher(EmbeddedFileExtractor extractor, AppDomainSetup setup)
        {
            this.Extractor = extractor;
            this.Setup = setup;
        }

        public void SetAppConfigFilePath(string filePath)
        {
            this.Setup.ConfigurationFile = filePath;
        }

        public void LoadConfigFromEmbeddedResources(string embeddedResource, string embeddedFileName)
        {
            string appConfigPath = this.Extractor.ExtractFileFromExeToDataDirectory(embeddedResource, embeddedFileName);
            this.Setup.ConfigurationFile = appConfigPath;
        }

        public void Start(string exeFileName, string appDomainName)
        {
            AppDomain appDomain = AppDomain.CreateDomain(appDomainName, null, this.Setup);
            appDomain.ExecuteAssemblyByName(exeFileName);
        }

    }
}
