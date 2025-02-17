namespace SimpleUi.Models
{
	public class UiWindowState
	{
		public static readonly UiWindowState IsActiveAndFocus = new(true, true);
		public static readonly UiWindowState IsActiveNotFocus = new(true, false);
		public static readonly UiWindowState NotActiveNotFocus = new(false, false);

		public readonly bool IsActive;
		public readonly bool InFocus;

		private UiWindowState(bool isActive, bool inFocus)
		{
			IsActive = isActive;
			InFocus = inFocus;
		}
	}
}