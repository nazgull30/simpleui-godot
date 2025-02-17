namespace SimpleUi.Signals
{
	public readonly struct SignalOpenRootWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }

		public SignalOpenRootWindow(EWindowLayer windowLayer)
		{
			WindowLayer = windowLayer;
		}
	}
}