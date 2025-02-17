using System;

namespace SimpleUi.Signals
{
	public readonly struct SignalOpenWindow : ISignalWindow
	{
		public EWindowLayer WindowLayer { get; }
		public readonly Type Type;
		public readonly string Name;

		public SignalOpenWindow(EWindowLayer windowLayer, Type type)
		{
			WindowLayer = windowLayer;
			Type = type;
			Name = null;
		}

		public SignalOpenWindow(EWindowLayer windowLayer, string name)
		{
			WindowLayer = windowLayer;
			Type = null;
			Name = name;
		}
	}
}