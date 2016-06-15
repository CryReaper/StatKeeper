using Rocket.API;

namespace StatKeeper
{
	public class Configuration : IRocketPluginConfiguration
	{
		public static Configuration Instance;

		public bool AdminView;

		public void LoadDefaults()
		{
			AdminView = false;
		}
	}
}
