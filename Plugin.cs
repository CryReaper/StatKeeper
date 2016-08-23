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
			U.Events.OnPlayerConnected += Events_OnPlayerConnected;
			U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
			Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;

			System.IO.Directory.CreateDirectory(Path.Combine(Directory, "Plugins/StatKeeper/Stats/"));
		}
		protected override void Unload()
		{
			U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
			U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
			base.Unload ();
		}
		private void Events_OnPlayerConnected(UnturnedPlayer player)
		{
            string statFile = Path.Combine(Directory, "Plugins/StatKeeper/Stats/", player.CSteamID + ".xml");

			if (!System.IO.File.Exists (statFile)) 
			{
				XmlTextWriter writer = new XmlTextWriter (statFile, System.Text.Encoding.UTF8);
				writer.WriteStartDocument (true);
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 2;
				writer.WriteStartElement ("Stats");
				writer.WriteStartElement ("PvP");
				StatKeeper.Utility.createPVPNode("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", writer);
				writer.WriteEndElement ();
				writer.WriteStartElement ("PvE");
				StatKeeper.Utility.createPVENode("0", "0", "0", "0", "0", "0", "0", writer);
				writer.WriteEndElement ();
				writer.WriteEndElement ();
				writer.WriteEndDocument ();
				writer.Close ();
			}
		}
		private void Events_OnPlayerDisconnected(UnturnedPlayer player)
		{

		}
		private void UnturnedPlayerEvents_OnPlayerDeath(Rocket.Unturned.Player.UnturnedPlayer player, SDG.Unturned.EDeathCause cause, SDG.Unturned.ELimb limb, Steamworks.CSteamID murderer)
		{
			UnturnedPlayer killer = Rocket.Unturned.Player.UnturnedPlayer.FromCSteamID(murderer);
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string filePath = currentPath + "\\Plugins\\StatKeeper\\Stats\\";
			string playerStatFile = filePath + player.CSteamID + ".xml";
			string killerStatFile = filePath + killer.CSteamID + ".xml";

			XmlDocument playerDoc = new XmlDocument();
			playerDoc.Load(playerStatFile);
			XmlElement playerPvPRoot = playerDoc.DocumentElement;
			XmlNodeList playerPvPNodes = playerPvPRoot.SelectNodes("/Stats/PvP");
			XmlElement playerPvERoot = playerDoc.DocumentElement;
			XmlNodeList playerPvENodes = playerPvERoot.SelectNodes("/Stats/PvE");

			XmlDocument killerDoc = new XmlDocument();
			killerDoc.Load(killerStatFile);
			XmlElement killerPvPRoot = killerDoc.DocumentElement;
			XmlNodeList killerPvPNodes = killerPvPRoot.SelectNodes("/Stats/PvP");
			XmlElement killerPvERoot = killerDoc.DocumentElement;
			XmlNodeList killerPvENodes = playerPvERoot.SelectNodes("/Stats/PvE");
				if (cause.ToString() == "ZOMBIE")
				{
					foreach (XmlNode node in playerPvENodes)
						{
							int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
							int zDeaths = int.Parse(node["ZombieDeaths"].InnerText);
							int newtDeaths = tDeaths + 1;
							int newzDeaths = zDeaths + 1;

							XmlNode totDeaths = playerPvERoot.SelectSingleNode("TotalDeaths");
							totDeaths.Value = newtDeaths + "";
							XmlNode zomDeaths = playerPvERoot.SelectSingleNode("ZombieDeaths");
							zomDeaths.Value = newzDeaths + "";

							playerDoc.Save(playerStatFile);
						}
				}
				else if (cause.ToString() == "GUN")
				{
					foreach (XmlNode node in playerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int gDeaths = int.Parse(node["GunDeaths"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newgDeaths = gDeaths + 1;
						float newKDRatio;
						if(tKills > 0 && tDeaths == 0)
						{
							newKDRatio = tKills/1;
						}
						else
						{
							newKDRatio = tKills/newtDeaths;
						}

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["GunDeaths"].InnerText = newgDeaths + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(playerStatFile);
					}
					foreach (XmlNode node in killerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int gKills = int.Parse(node["GunKills"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtKills = tKills + 1;
						int newgKills = gKills + 1;
						float newKDRatio;
						if(newtKills > 0 && tDeaths == 0)
						{
							newKDRatio = newtKills/1;
						}
						else
						{
							newKDRatio = newtKills/tDeaths;
						}

						node["TotalKills"].InnerText = newtKills + "";
						node["GunKills"].InnerText = newgKills + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(killerStatFile);
					}
				}
				else if (cause.ToString() == "MELEE")
				{
					foreach (XmlNode node in playerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int mDeaths = int.Parse(node["MeleeDeaths"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newmDeaths = mDeaths + 1;
						float newKDRatio;
						if(tKills > 0 && tDeaths == 0)
						{
							newKDRatio = tKills/1;
						}
						else
						{
							newKDRatio = tKills/newtDeaths;
						}

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["MeleeDeaths"].InnerText = newmDeaths + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(playerStatFile);
					}
					foreach (XmlNode node in killerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int mKills = int.Parse(node["MeleeKills"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtKills = tKills + 1;
						int newmKills = mKills + 1;
						float newKDRatio;
						if(newtKills > 0 && tDeaths == 0)
						{
							newKDRatio = newtKills/1;
						}
						else
						{
							newKDRatio = newtKills/tDeaths;
						}

						node["TotalKills"].InnerText = newtKills + "";
						node["MeleeKills"].InnerText = newmKills + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(killerStatFile);
					}
				}
				else if (cause.ToString() == "PUNCH")
				{
					foreach (XmlNode node in playerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int pDeaths = int.Parse(node["PunchDeaths"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newpDeaths = pDeaths + 1;
						float newKDRatio;
						if(tKills > 0 && tDeaths == 0)
						{
							newKDRatio = tKills/1;
						}
						else
						{
							newKDRatio = tKills/newtDeaths;
						}

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["PunchDeaths"].InnerText = newpDeaths + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(playerStatFile);
					}
					foreach (XmlNode node in killerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int pKills = int.Parse(node["PunchKills"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtKills = tKills + 1;
						int newpKills = pKills + 1;
						float newKDRatio;
						if(newtKills > 0 && tDeaths == 0)
						{
							newKDRatio = newtKills/1;
						}
						else
						{
							newKDRatio = newtKills/tDeaths;
						}

						node["TotalKills"].InnerText = newtKills + "";
						node["PunchKills"].InnerText = newpKills + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(killerStatFile);
					}
				}
				else if (cause.ToString() == "ROADKILL")
				{
					foreach (XmlNode node in playerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int rDeaths = int.Parse(node["RoadDeaths"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newrDeaths = rDeaths + 1;
						float newKDRatio;
						if(tKills > 0 && tDeaths == 0)
						{
							newKDRatio = tKills/1;
						}
						else
						{
							newKDRatio = tKills/newtDeaths;
						}

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["RoadDeaths"].InnerText = newrDeaths + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(playerStatFile);
					}
					foreach (XmlNode node in killerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int rKills = int.Parse(node["RoadKills"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtKills = tKills + 1;
						int newrKills = rKills + 1;
						float newKDRatio;
						if(newtKills > 0 && tDeaths == 0)
						{
							newKDRatio = newtKills/1;
						}
						else
						{
							newKDRatio = newtKills/tDeaths;
						}

						node["TotalKills"].InnerText = newtKills + "";
						node["RoadKills"].InnerText = newrKills + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(killerStatFile);
					}
				}
				else if (cause.ToString() == "VEHICLE")
				{
					foreach (XmlNode node in playerPvENodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int vDeaths = int.Parse(node["VehicleDeaths"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newvDeaths = vDeaths + 1;

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["VehicleDeaths"].InnerText = newvDeaths + "";

						playerDoc.Save(playerStatFile);
					}
				}
				else if (cause.ToString() == "FOOD")
				{
					foreach (XmlNode node in playerPvENodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int fDeaths = int.Parse(node["FoodDeaths"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newfDeaths = fDeaths + 1;

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["FoodDeaths"].InnerText = newfDeaths + "";

						playerDoc.Save(playerStatFile);
					}
				}
				else if (cause.ToString() == "WATER")
				{
					foreach (XmlNode node in playerPvENodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int wDeaths = int.Parse(node["FoodDeaths"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newwDeaths = wDeaths + 1;

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["WaterDeaths"].InnerText = newwDeaths + "";

						playerDoc.Save(playerStatFile);
					}
				}
				else if (cause.ToString() == "INFECTION")
				{
					foreach (XmlNode node in playerPvENodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int iDeaths = int.Parse(node["InfectionDeaths"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newiDeaths = iDeaths + 1;

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["InfectionDeaths"].InnerText = newiDeaths + "";

						playerDoc.Save(playerStatFile);
					}
				}
				else if (cause.ToString() == "BLEEDING")
				{
					foreach (XmlNode node in playerPvENodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int bDeaths = int.Parse(node["BleedingDeaths"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newbDeaths = bDeaths + 1;

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["BleedingDeaths"].InnerText = newbDeaths + "";

						playerDoc.Save(playerStatFile);
					}
				}
				else if (cause.ToString() == "SUICIDE")
				{
					foreach (XmlNode node in playerPvPNodes)
					{
						int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
						int sDeaths = int.Parse(node["SuicideDeaths"].InnerText);
						int tKills = int.Parse(node["TotalKills"].InnerText);
						int kdRatio = int.Parse(node["KDRatio"].InnerText);
						int newtDeaths = tDeaths + 1;
						int newsDeaths = sDeaths + 1;
						float newKDRatio = tKills/newtDeaths;

						node["TotalDeaths"].InnerText = newtDeaths + "";
						node["SuicideDeaths"].InnerText = newsDeaths + "";
						node["KDRatio"].InnerText = newKDRatio + "";

						playerDoc.Save(playerStatFile);
					}
				}
			}
	}
}

