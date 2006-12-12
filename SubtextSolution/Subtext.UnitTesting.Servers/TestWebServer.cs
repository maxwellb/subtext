using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.VisualStudio.WebHost;

namespace Subtext.UnitTesting.Servers
{
	public sealed class TestWebServer : IDisposable
	{
		private Server webServer;
		private int webServerPort;
		private string webServerVDir;
		private string sourceBinDir = AppDomain.CurrentDomain.BaseDirectory;
		private string webRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebRoot");
		private string webBinDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
		private string webServerUrl; //built in Start

		public TestWebServer() : this(8085, "/")
		{
		}

		public TestWebServer(int port, string virtualDir)
		{
			this.webServerPort = port;
			this.webServerVDir = virtualDir;
		}

		/// <summary>
		/// Starts the webserver and returns the URL.
		/// </summary>
		public Uri Start()
		{
			//NOTE: Cassini is going to load itself AGAIN into another AppDomain,
			// and will be getting it's Assembliesfrom the BIN, including another copy of itself!
			// Therefore we need to do this step FIRST because I've removed Cassini from the GAC
			//Copy our assemblies down into the web server's BIN folder
			if (!Directory.Exists(webRoot))
				Directory.CreateDirectory(webRoot);

			if (!Directory.Exists(webBinDir))
				Directory.CreateDirectory(webBinDir);

			CopyAssembliesToWebServerBinDirectory();

			//Start the internal Web Server
			webServer = new Server(webServerPort, webServerVDir, this.webRoot);
			webServerUrl = String.Format("http://localhost:{0}{1}", webServerPort, webServerVDir);
			
			webServer.Start();
			Debug.WriteLine(String.Format("Web Server started on port {0} with VDir {1} in physical directory {2}", webServerPort, webServerVDir, this.webRoot));
			return new Uri(webServerUrl);
		}

		private void CopyAssembliesToWebServerBinDirectory()
		{
			foreach (string file in Directory.GetFiles(this.sourceBinDir, "*.dll"))
			{
				string newFile = Path.Combine(this.webBinDir, Path.GetFileName(file));
				if (File.Exists(newFile))
				{
					File.Delete(newFile);
				}
				File.Copy(file, newFile);
			}
		}

		/// <summary>
		/// Makes a request to the web server.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public string GetPage(string page)
		{
			WebClient client = new WebClient();
			string url = new Uri(new Uri(this.webServerUrl), page).ToString();
			using (StreamReader reader = new StreamReader(client.OpenRead(url)))
			{
				string result = reader.ReadToEnd();
				return result;
			}
		}

		/// <summary>
		/// Extracts a resources such as an html file or aspx page to the webroot directory
		/// and returns the filepath.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <param name="destinationFileName">Name of the destination file.</param>
		/// <returns></returns>
		public string ExtractResource(string resourceName, string destinationFileName)
		{
			Assembly a = Assembly.GetCallingAssembly();
			string filePath;
			using (Stream stream = a.GetManifestResourceStream(resourceName))
			{
				filePath = Path.Combine(this.webRoot, destinationFileName);
				using (StreamWriter outfile = File.CreateText(filePath))
				{
					using (StreamReader infile = new StreamReader(stream))
					{
						outfile.Write(infile.ReadToEnd());
					}
				}
			}
			return filePath;
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			Dispose();
		}

		~TestWebServer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		/// <remarks>
		/// If we unseal this class, make sure this is protected virtual.
		/// </remarks>
		///<filterpriority>2</filterpriority>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				ReleaseManagedResources();
			}
		}

		private void ReleaseManagedResources()
		{
			if(this.webServer != null)
			{
				this.webServer.Stop();
				this.webServer = null;
			}

			if (Directory.Exists(this.webRoot))
				Directory.Delete(this.webRoot, true);
		}
	}
}