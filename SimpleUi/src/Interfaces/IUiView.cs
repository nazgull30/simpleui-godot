using Godot;
using Godot.Collections;

namespace SimpleUi.Interfaces
{
	public interface IUiView
	{
		bool IsShow { get; }

		void Show();
		void Hide();
		void SetOrder(int index);
		Array<Control> GetUiElements();
		void SetParent(Control parent);
		void Destroy();
	}
}