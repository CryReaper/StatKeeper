using System.Xml;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Rocket.Core.Plugins;
using System.IO;

namespace StatKeeper
{
	public class Plugin : RocketPlugin<Configuration>
	{
		public static Plugin Instance;
	
		protected override void Load()
		{
			Instance = this;
			Logger.Log("StatKeeper has been loaded!");
			System.IO.Directory.CreateDirectory(Path.Combine(Directory, "Stats/"));
		}
    }
}

