using System.Collections.Generic;
using Godot;
using Godot.Collections;
using SimpleUi.Interfaces;
using SimpleUi.Models;
using VContainer;

namespace SimpleUi
{
	public abstract class WindowBase : Window
	{
		private readonly List<IUiController> _controllers = [];

		[Inject] protected IObjectResolver _resolver;

		[Inject]
		protected abstract void AddControllers();

		protected void AddController<TController>()
			where TController : IUiController
		{
			var controller = _resolver.Resolve<TController>();
			_controllers.Add(controller);
		}

		public override void SetState(UiWindowState state)
		{
			for (var i = 0; i < _controllers.Count; i++)
				_controllers[i].SetState(new UiControllerState(state.IsActive, state.InFocus, i));
			ProcessState();
		}

		public override void Back()
		{
			for (var i = 0; i < _controllers.Count; i++)
				_controllers[i].Back();
			ProcessState();
		}

		private void ProcessState()
		{
			for (var i = 0; i < _controllers.Count; i++)
				_controllers[i].ProcessStateOrder();
			for (var i = 0; i < _controllers.Count; i++)
				_controllers[i].ProcessState();
		}

		public override Array<Control> GetUiElements()
		{
			var list = new Array<Control>();
			foreach (var t in _controllers)
				list.AddRange(t.GetUiElements());

			return list;
		}
	}
}