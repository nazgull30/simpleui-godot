using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public readonly struct SignalCloseWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }
		public readonly IWindow Window;

		public SignalCloseWindow(EWindowLayer windowLayer, IWindow window)
		{
			WindowLayer = windowLayer;
			Window = window;
		}
	}
}