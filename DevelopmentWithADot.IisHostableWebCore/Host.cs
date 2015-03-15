using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DevelopmentWithADot.IisHostableWebCore
{
	public class Host : IDisposable
	{
		private static readonly String FrameworkDirectory = RuntimeEnvironment.GetRuntimeDirectory();
		private static readonly String RootWebConfigPath = Environment.ExpandEnvironmentVariables(Path.Combine(FrameworkDirectory, @"Config\Web.config"));

		public Host(String physicalPath, Int32 port)
		{
			this.ApplicationHostConfigurationPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".config");
			this.PhysicalPath = physicalPath;
			this.Port = port;

			var applicationHostConfigurationContent = File.ReadAllText("ApplicationHost.config");
			var text = String.Format(applicationHostConfigurationContent, this.PhysicalPath, this.Port);

			File.WriteAllText(this.ApplicationHostConfigurationPath, text);
		}

		~Host()
		{
			this.Dispose(false);
		}

		public String ApplicationHostConfigurationPath
		{
			get;
			private set;
		}

		public Int32 Port
		{
			get;
			private set;
		}

		public String PhysicalPath
		{
			get;
			private set;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(Boolean disposing)
		{
			this.Stop();
		}

		public void Start()
		{
			if (IisHostableWebCoreEngine.IsActivated == false)
			{
				IisHostableWebCoreEngine.Activate(this.ApplicationHostConfigurationPath, RootWebConfigPath, Guid.NewGuid().ToString());
			}
		}

		public void Stop()
		{
			if (IisHostableWebCoreEngine.IsActivated == true)
			{
				IisHostableWebCoreEngine.Shutdown(false);

				this.PhysicalPath = String.Empty;
				this.Port = 0;

				File.Delete(this.ApplicationHostConfigurationPath);

				this.ApplicationHostConfigurationPath = String.Empty;
			}
		}

		private static class IisHostableWebCoreEngine
		{
			private delegate Int32 FnWebCoreActivate([In, MarshalAs(UnmanagedType.LPWStr)] String appHostConfig, [In, MarshalAs(UnmanagedType.LPWStr)] String rootWebConfig, [In, MarshalAs(UnmanagedType.LPWStr)] String instanceName);
			private delegate Int32 FnWebCoreShutdown(Boolean immediate);

			private const String HostableWebCorePath = @"%WinDir%\System32\InetSrv\HWebCore.dll";
			private static readonly IntPtr HostableWebCoreLibrary = LoadLibrary(Environment.ExpandEnvironmentVariables(HostableWebCorePath));

			private static readonly IntPtr WebCoreActivateAddress = GetProcAddress(HostableWebCoreLibrary, "WebCoreActivate");
			private static readonly FnWebCoreActivate WebCoreActivate = Marshal.GetDelegateForFunctionPointer(WebCoreActivateAddress, typeof(FnWebCoreActivate)) as FnWebCoreActivate;

			private static readonly IntPtr WebCoreShutdownAddress = GetProcAddress(HostableWebCoreLibrary, "WebCoreShutdown");
			private static readonly FnWebCoreShutdown WebCoreShutdown = Marshal.GetDelegateForFunctionPointer(WebCoreShutdownAddress, typeof(FnWebCoreShutdown)) as FnWebCoreShutdown;

			internal static Boolean IsActivated
			{
				get;
				private set;
			}

			internal static void Activate(String appHostConfig, String rootWebConfig, String instanceName)
			{
				var result = WebCoreActivate(appHostConfig, rootWebConfig, instanceName);

				if (result != 0)
				{
					Marshal.ThrowExceptionForHR(result);
				}

				IsActivated = true;
			}

			internal static void Shutdown(Boolean immediate)
			{
				if (IsActivated == true)
				{
					WebCoreShutdown(immediate);
					IsActivated = false;
				}
			}

			[DllImport("Kernel32.dll")]
			private static extern IntPtr LoadLibrary(String dllname);

			[DllImport("Kernel32.dll")]
			private static extern IntPtr GetProcAddress(IntPtr hModule, String procname);
		}
	}
}
