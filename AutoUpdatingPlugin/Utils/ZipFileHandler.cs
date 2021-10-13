using MelonLoader.ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace AutoUpdatingPlugin
{
    internal static class ZipFileHandler
    {
        internal static void ExtractZipFilesInDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.zip");
            foreach (string eachFile in files)
            {
                ExtractZipFile(eachFile);
            }
        }
        private static void ExtractZipFile(string zipFilePath)
        {
            var fileStream = File.OpenRead(zipFilePath);
            var zipInputStream = new ZipInputStream(fileStream);
            ZipEntry entry;
            while ((entry = zipInputStream.GetNextEntry()) != null)
            {
                string internalPath = entry.Name;

                using (var unzippedFileStream = new MemoryStream())
                {
                    int size = 0;
                    byte[] buffer = new byte[4096];
                    while (true)
                    {
                        size = zipInputStream.Read(buffer, 0, buffer.Length);
                        if (size > 0)
                            unzippedFileStream.Write(buffer, 0, size);
                        else
                            break;
                    }
                    string fullPath = Path.Combine(Path.GetDirectoryName(zipFilePath), internalPath);
                    File.WriteAllBytes(fullPath, unzippedFileStream.ToArray());
                }
            }
            File.Delete(zipFilePath);
        }
    }
}
