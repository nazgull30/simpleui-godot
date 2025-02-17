namespace SimpleUi.Models
{
	public class UiControllerState(bool isActive, bool inFocus, int order)
	{
		public bool IsActive = isActive;
		public bool InFocus = inFocus;
		public int Order = order;
	}
}