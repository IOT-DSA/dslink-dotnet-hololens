using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using UnityPlayer;

namespace DSHoloLens
{
	public class App : IFrameworkView, IFrameworkViewSource
	{
		private WinRTBridge.WinRTBridge m_Bridge;
		private AppCallbacks m_AppCallbacks;

		public App()
		{
			m_AppCallbacks = new AppCallbacks();

			// Allow clients of this class to append their own callbacks.
			AddAppCallbacks(m_AppCallbacks);
		}

		public virtual void Initialize(CoreApplicationView applicationView)
		{
			applicationView.Activated += ApplicationView_Activated;
			CoreApplication.Suspending += CoreApplication_Suspending;

		    HoloLensDSLink.Start();

			// Setup scripting bridge
			m_Bridge = new WinRTBridge.WinRTBridge();
			m_AppCallbacks.SetBridge(m_Bridge);

			m_AppCallbacks.SetCoreApplicationViewEvents(applicationView);
		}

		/// <summary>
		/// This is where apps can hook up any additional setup they need to do before Unity intializes.
		/// </summary>
		/// <param name="appCallbacks"></param>
		protected virtual void AddAppCallbacks(AppCallbacks appCallbacks)
		{
		}

		private static void CoreApplication_Suspending(object sender, SuspendingEventArgs e)
		{

		}

		private static void ApplicationView_Activated(CoreApplicationView sender, IActivatedEventArgs args)
		{
			CoreWindow.GetForCurrentThread().Activate();
		}

		public void SetWindow(CoreWindow coreWindow)
		{
		    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
			if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
#pragma warning disable 4014
			{
				StatusBar.GetForCurrentView().HideAsync();
			}
#pragma warning restore 4014

			m_AppCallbacks.SetCoreWindowEvents(coreWindow);
			m_AppCallbacks.InitializeD3DWindow();
		}

		public void Load(string entryPoint)
		{
		}

		public void Run()
		{
			m_AppCallbacks.Run();
		}

		public void Uninitialize()
		{
		}

		[MTAThread]
		private static void Main(string[] args)
		{
			var app = new App();
			CoreApplication.Run(app);
		}

		public IFrameworkView CreateView()
		{
			return this;
		}
	}
}
