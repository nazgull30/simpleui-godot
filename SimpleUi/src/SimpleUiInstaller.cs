using Godot;
using VContainer;
using VContainer.Unity;

namespace SimpleUi
{
	public partial class SimpleUiInstaller : MonoInstaller
	{
		[Export]
		private EWindowLayer windowLayer;

		public override void Install(IContainerBuilder builder)
		{
			builder.BindWindowsController<WindowsController>(windowLayer);
		}
	}
}
