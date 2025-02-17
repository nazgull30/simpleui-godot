using Godot;
using Godot.Collections;
using SimpleUi.Interfaces;
using VContainer;

namespace SimpleUi.Abstracts
{
	public abstract partial class UiView : Control, IUiView
	{
		public bool IsShow { get; private set; }

		[Inject]
		protected virtual void Resolve(IObjectResolver resolver)
		{
			
		}
		
		void IUiView.Show()
		{
			Show();
			IsShow = true;
			OnShow();
		}

		protected virtual void OnShow()
		{
		}

		void IUiView.Hide()
		{
			Hide();
			IsShow = false;
			OnHide();
		}

		protected virtual void OnHide()
		{
		}

		public Array<Control> GetUiElements()
		{
			var foundNodes = GetChildren();
			var results = new Array<Control>();
			foreach (var nd in foundNodes)
				if (nd is Control control) 
					results.Add(control);
			return results;
		}

		void IUiView.SetParent(Control parent)
		{
			parent.AddChild(this);
		}

		void IUiView.SetOrder(int index)
		{
			var parent = GetParent();
			if (parent == null)
				return;
			var childCount = parent.GetChildCount() - 1;
			parent.MoveChild(this, childCount - index);
		}

		void IUiView.Destroy()
		{
			QueueFree();
		}
	}
}