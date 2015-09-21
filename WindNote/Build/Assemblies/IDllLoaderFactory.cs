using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindNote.Build.EmbeddedResources;

namespace WindNote.Assemblies
{
    public interface IDllLoaderFactory
    {
        UnmanagedDllLoader CreateUnmanagedDllLoader();
    }

    public class DllLoaderFactory : IDllLoaderFactory
    {
        public UnmanagedDllLoader CreateUnmanagedDllLoader()
        {
            EmbeddedFileExtractor extractor = new EmbeddedFileExtractor();
            return new UnmanagedDllLoader(extractor);
        }
    }

}
