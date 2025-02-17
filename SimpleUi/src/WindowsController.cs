using System;
using System.Collections.Generic;
using System.Linq;
using PdEventBus.Impls;
using PdEventBus.Utils;
using SimpleUi.Interfaces;
using SimpleUi.Models;
using SimpleUi.Signals;
using VContainer.Unity;

namespace SimpleUi
{
	public class WindowsController(
		LifetimeScope container,
		IReadOnlyList<IWindow> windows,
		WindowState windowState,
		EWindowLayer windowLayer)
		: IWindowsController, IInitializable, IDisposable
	{
		private readonly Stack<IWindow> _windowsStack = new();
		private readonly CompositeDisposable _disposables = new();

		private IWindow _window;

		public Stack<IWindow> Windows => _windowsStack;

		public void Initialize()
		{
			Event<SignalOpenWindow>.Instance
				.Where(v => v.WindowLayer == windowLayer)
				.Subscribe(OnOpen).AddTo(_disposables);
			Event<SignalBackWindow>.Instance
				.Where(v => v.WindowLayer == windowLayer)
				.Subscribe(_ => OnBack()).AddTo(_disposables);
			Event<SignalOpenRootWindow>.Instance
				.Where(v => v.WindowLayer == windowLayer)
				.Subscribe(OnOpenRootWindow).AddTo(_disposables);
		}

		public void Dispose() => _disposables.Dispose();

		private void OnOpen(SignalOpenWindow signal)
		{
			IWindow window;
			if (signal.Type != null)
				window = container.Container.Resolve(signal.Type) as IWindow;
			else
				window = windows.First(f => f.Name == signal.Name);
			Open(window);
		}

		private void Open(IWindow window)
		{
			var isNextWindowPopUp = window is IPopUp;
			var currentWindow = _windowsStack.Count > 0 ? _windowsStack.Peek() : null;
			if (currentWindow != null)
			{
				var isCurrentWindowPopUp = currentWindow is IPopUp;
				var isCurrentWindowNoneHidden = currentWindow is INoneHidden;
				if (isCurrentWindowPopUp)
				{
					if (!isNextWindowPopUp)
					{
						var openedWindows = GetPreviouslyOpenedWindows();
						var popupsOpened = GetPopupsOpened(openedWindows);
						var last = openedWindows.Last();
						last.SetState(UiWindowState.NotActiveNotFocus);

						foreach (var openedPopup in popupsOpened)
						{
							openedPopup.SetState(UiWindowState.NotActiveNotFocus);
						}
					}
					else
						currentWindow.SetState(isCurrentWindowNoneHidden
							? UiWindowState.IsActiveNotFocus
							: UiWindowState.NotActiveNotFocus);
				}
				else if (isNextWindowPopUp)
					_window?.SetState(UiWindowState.IsActiveNotFocus);
				else
					_window?.SetState(isCurrentWindowNoneHidden
						? UiWindowState.IsActiveNotFocus
						: UiWindowState.NotActiveNotFocus);
			}

			_windowsStack.Push(window);
			windowState.CurrentWindowName = window.Name;
			window.SetState(UiWindowState.IsActiveAndFocus);
			Event<SignalShowWindow>.Fire(new SignalShowWindow(windowLayer, window));
			ActiveAndFocus(window, isNextWindowPopUp);
		}

		private void OnBack()
		{
			if (_windowsStack.Count == 0)
				return;

			var currentWindow = _windowsStack.Pop();
			currentWindow.Back();
			Event<SignalCloseWindow>.Fire(new SignalCloseWindow(windowLayer, currentWindow));
			OpenPreviousWindows();
		}

		private void OpenPreviousWindows()
		{
			if (_windowsStack.Count == 0)
				return;

			var openedWindows = GetPreviouslyOpenedWindows();
			var popupsOpened = GetPopupsOpened(openedWindows);
			var firstWindow = GetFirstWindow();
			var isFirstPopUp = false;

			var isNoPopups = popupsOpened.Count == 0;
			var isOtherWindow = firstWindow != _window;
			if (isOtherWindow || isNoPopups)
			{
				firstWindow = openedWindows.Last();
				firstWindow.Back();
				_window = firstWindow;
			}

			if (!isNoPopups)
			{
				var window = popupsOpened.Last();
				window.Back();
				firstWindow = window;
				isFirstPopUp = true;

				if (isOtherWindow)
				{
					var nonHiddenPopUps = popupsOpened.Take(popupsOpened.Count - 1);
					foreach (var nonHiddenPopUp in nonHiddenPopUps)
						nonHiddenPopUp.Back();
				}
			}

			windowState.CurrentWindowName = firstWindow.Name;
			ActiveAndFocus(firstWindow, isFirstPopUp);
		}

		private void ActiveAndFocus(IWindow window, bool isPopUp)
		{
			if (!isPopUp)
				_window = window;

			Event<SignalActiveWindow>.Fire(new SignalActiveWindow(windowLayer, window));
			Event<SignalFocusWindow>.Fire(new SignalFocusWindow(windowLayer, window));
		}

		private List<IWindow> GetPreviouslyOpenedWindows()
		{
			var windows = new List<IWindow>();

			var hasWindow = false;
			foreach (var window in _windowsStack)
			{
				var isPopUp = window is IPopUp;
				if (isPopUp)
				{
					if (hasWindow)
						break;

					windows.Add(window);
					continue;
				}

				if (hasWindow)
					break;
				windows.Add(window);
				hasWindow = true;
			}

			return windows;
		}

		private Stack<IWindow> GetPopupsOpened(List<IWindow> windows)
		{
			var stack = new Stack<IWindow>();

			var hasPopup = false;
			for (var i = 0; i < windows.Count; i++)
			{
				var window = windows[i];
				var isPopUp = window is IPopUp;
				if (!isPopUp)
					break;

				if (hasPopup && !(window is INoneHidden))
					continue;

				stack.Push(window);
				hasPopup = true;
			}

			return stack;
		}

		private IWindow GetFirstWindow()
		{
			foreach (var element in _windowsStack)
			{
				if (element is IPopUp)
					continue;
				return element;
			}

			return null;
		}

		private void OnOpenRootWindow(SignalOpenRootWindow obj)
		{
			while (_windowsStack.Count > 1)
			{
				OnBack();
			}
		}

		public void Reset()
		{
			while (_windowsStack.Count > 0)
			{
				OnBack();
			}

			_window = null;
		}
	}
}