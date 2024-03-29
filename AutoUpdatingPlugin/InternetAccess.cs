﻿using System;
using System.Net;
using System.Threading;

namespace AutoUpdatingPlugin
{
	internal static class InternetAccess
	{
		internal const string DownloadURL = @"https://github.com/ds5678/AutoUpdatingPlugin/releases/latest/download/AutoUpdatingPlugin.dll";
		private const string VersionURL = @"https://raw.githubusercontent.com/ds5678/AutoUpdatingPlugin/master/Version.json";

		internal static string GetVersionJsonText()
		{
			string apiResponse = "";
			using (WebClient? client = new WebClient())
			{
				client.Headers["User-Agent"] = "AutoUpdatingPlugin";
				apiResponse = client.DownloadString(VersionURL);
			}
			return apiResponse;
		}

		internal static bool TryDownloadFile(string downloadLink, out byte[]? data)
		{
			bool errored = false;
			using WebClient? client = new WebClient();
			bool downloading;
			byte[]? buffer;
			client.DownloadDataCompleted += (sender, e) =>
			{
				if (e.Error != null)
				{
					Logger.Error("Failed to download a newer version of the Auto Updating Plugin:\n" + e.Error);
					errored = true;
				}
				else
				{
					buffer = e.Result;
				}

				downloading = false;
			};
			downloading = true;
			buffer = null;
			client.DownloadDataAsync(new Uri(downloadLink));

			while (downloading)
			{
				Thread.Sleep(50);
			}

			data = buffer;
			return !errored;
		}
	}
}
