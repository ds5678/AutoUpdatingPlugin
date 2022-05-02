﻿using MelonLoader.ICSharpCode.SharpZipLib.Zip;
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
			string targetDir = Path.GetDirectoryName(zipFilePath);
			FastZip fastZip = new FastZip();
			string fileFilter = null;
			// Will always overwrite if target filenames already exist
			fastZip.ExtractZip(zipFilePath, targetDir, fileFilter);
			File.Delete(zipFilePath);
		}
	}
}
