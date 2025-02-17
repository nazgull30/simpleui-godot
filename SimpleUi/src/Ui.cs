using System;
using PdEventBus.Impls;
using SimpleUi.Signals;

namespace SimpleUi
{
	public static class Ui
	{
		public static void OpenWindow<TWindow>(EWindowLayer windowLayer = EWindowLayer.Local)
			where TWindow : Window
			=> Event<SignalOpenWindow>.Fire(new SignalOpenWindow(windowLayer, typeof(TWindow)));

		public static void OpenWindow(Type type, EWindowLayer windowLayer = EWindowLayer.Local)
			=> Event<SignalOpenWindow>.Fire(new SignalOpenWindow(windowLayer, type));
		
		public static void OpenWindow(string name, EWindowLayer windowLayer = EWindowLayer.Local)
			=> Event<SignalOpenWindow>.Fire(new SignalOpenWindow(windowLayer, name));
		
				
		public static void Back(EWindowLayer windowLayer = EWindowLayer.Local)
			=> Event<SignalBackWindow>.Fire(new SignalBackWindow(windowLayer));
	}
}