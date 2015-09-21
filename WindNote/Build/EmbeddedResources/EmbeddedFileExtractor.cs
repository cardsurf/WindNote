using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindNote.Build.EmbeddedResources
{

    public class EmbeddedFileExtractor
    {

       public string ExtractFileFromExeToTempDirectory(string embeddedResourceName, string embeddedFileName)
       {
            string directory = Path.GetTempPath();
            string filePath = Path.Combine(directory, embeddedFileName);
            return this.ExtractFileFromEmbeddedResources(embeddedResourceName, filePath);
       }

       public string ExtractFileFromExeToDataDirectory(string embeddedResourceName, string embeddedFileName)
       {
           string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as String;
           if(dataDirectory == null)
           {
               throw new Exception("AppDomain.CurrentDomain.GetData('DataDirectory') is null. To set path of 'DataDirectory' use: AppDomain.CurrentDomain.SetData('DataDirectory', path);");
           }
           string filePath = Path.Combine(dataDirectory, embeddedFileName);
           return this.ExtractFileFromEmbeddedResources(embeddedResourceName, filePath);
       }

       private string ExtractFileFromEmbeddedResources(string embeddedResourceName, string filePath)
       {
            byte[] content = this.ExtractFileContentEmbeddedResources(embeddedResourceName);
            if (!this.FileExtracted(filePath, content))
            {
                File.WriteAllBytes(filePath, content);
            }
            return filePath;
        }

       private byte[] ExtractFileContentEmbeddedResources(string embeddedResourceName)
       {
           byte[] content = null;
           Assembly assembly = Assembly.GetExecutingAssembly();

           using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
           {
               if (stream == null)
               {
                   throw new Exception(embeddedResourceName + " is not found in Embedded Resources.");
               }

               int bytesInFile = (int)stream.Length;
               content = new byte[bytesInFile];
               stream.Read(content, 0, bytesInFile);
           }
           return content;
       }

       private bool FileExtracted(string filePath, byte[] embeddedFileContent)
       {
           if (File.Exists(filePath))
           {
               byte[] diskFileContent = File.ReadAllBytes(filePath);
               bool result = this.HashFilesEqual(embeddedFileContent, diskFileContent);
               return result;
           }

           return false;
       }

       private bool HashFilesEqual(byte[] contentFile1, byte[] contentFile2)
       {
           using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
           {
               string hashFile1 = BitConverter.ToString(sha1.ComputeHash(contentFile1)).Replace("-", string.Empty);
               string hashFile2 = BitConverter.ToString(sha1.ComputeHash(contentFile2)).Replace("-", string.Empty);
               return hashFile1 == hashFile2;
           }
       }

    }

}
