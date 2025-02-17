using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public readonly struct SignalFocusWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }
		public readonly IWindow Window;

		public SignalFocusWindow(EWindowLayer windowLayer, IWindow window)
		{
			WindowLayer = windowLayer;
			Window = window;
		}
	}
}