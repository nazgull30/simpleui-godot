using SimpleUi.Interfaces;

namespace SimpleUi.Signals
{
	public readonly struct SignalShowWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }
		public readonly IWindow Window;

		public SignalShowWindow( EWindowLayer windowLayer, IWindow window)
		{
			WindowLayer = windowLayer;
			Window = window;
		}
	}
}