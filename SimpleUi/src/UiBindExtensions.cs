using Godot;
using SimpleUi.Interfaces;
using VContainer;
using VContainer.Unity;

namespace SimpleUi
{
	public static class UiBindExtensions
	{
		public static void BindUiView<TController, TView>(this IContainerBuilder container, PackedScene viewPrefab, Node parent)
			where TView : IUiView
			where TController : IUiController
		{
			container.Register<TController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			container.RegisterComponentInNewPrefab<TView>(viewPrefab, Lifetime.Singleton)
				.UnderTransform(parent)
				.OnInstantiated((o) => ((Control) o).Hide());
		}
		
		public static void BindUiView<TController, TView>(this IContainerBuilder container, TView view)
			where TView : IUiView
			where TController : IUiController
		{
			container.Register<TController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
			container.RegisterInstance(view).AsImplementedInterfaces().AsSelf();
			view.Hide();
		}

		public static void BindWindowsController<T>(this IContainerBuilder container, EWindowLayer windowLayer)
			where T : IWindowsController, IInitializable
		{
			//TODO: BindInitializableExecutionOrder
			// container.BindInitializableExecutionOrder<T>(-1000); 
			container.RegisterEntryPoint<T>().AsSelf().WithParameter(windowLayer);
			var windowState = new WindowState();
			container.RegisterInstance(windowState);
		}
	}
}