using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public readonly struct SignalActiveWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }
		public readonly IWindow Window;

		public SignalActiveWindow(EWindowLayer windowLayer, IWindow window)
		{
			Window = window;
			WindowLayer = windowLayer;
		}
	}
}