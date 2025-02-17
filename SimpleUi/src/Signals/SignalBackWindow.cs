namespace SimpleUi.Signals
{
	public readonly struct SignalBackWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }

		public SignalBackWindow(EWindowLayer windowLayer)
		{
			WindowLayer = windowLayer;
		}
	}
}