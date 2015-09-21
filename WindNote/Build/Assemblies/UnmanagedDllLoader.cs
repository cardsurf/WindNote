using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindNote.Build.EmbeddedResources;

namespace WindNote.Assemblies
{

    public class UnmanagedDllLoader
    {
        private EmbeddedFileExtractor Extractor;

        public UnmanagedDllLoader(EmbeddedFileExtractor extractor)
        {
            this.Extractor = extractor;
        }

        public void LoadFromFile(string filePath)
        {
            IntPtr pointer = DllNativeMethods.LoadLibrary(filePath);
            if (pointer == IntPtr.Zero)
            {
                throw new DllNotFoundException("Unable to load .dll library: " + filePath);
            }
        }

        public void LoadFromEmbeddedResources(string embeddedResource, string embeddedFileName)
        {
            string dll = this.Extractor.ExtractFileFromExeToDataDirectory(embeddedResource, embeddedFileName);
            this.LoadFromFile(dll);
        }
    }

}
