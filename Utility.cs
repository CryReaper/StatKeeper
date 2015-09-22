using System;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace StatKeeper
{
	public class Utility
	{
		public static Utility Instance;

		public static void createPVPNode(string tKills, string gKills, string mKills, string pKills, string rKills, string tDeaths,
			string gDeaths, string mDeaths, string pDeaths, string rDeaths, string sDeaths, string kdRatio, XmlTextWriter writer)
		{
			writer.WriteStartElement("TotalKills");
			writer.WriteString(tKills);
			writer.WriteEndElement();
			writer.WriteStartElement("GunKills");
			writer.WriteString(gKills);
			writer.WriteEndElement();
			writer.WriteStartElement("MeleeKills");
			writer.WriteString(mKills);
			writer.WriteEndElement();
			writer.WriteStartElement("PunchKills");
			writer.WriteString(pKills);
			writer.WriteEndElement();
			writer.WriteStartElement("RoadKills");
			writer.WriteString(rKills);
			writer.WriteEndElement();
			writer.WriteStartElement("TotalDeaths");
			writer.WriteString(tDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("GunDeaths");
			writer.WriteString(gDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("MeleeDeaths");
			writer.WriteString(mDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("PunchDeaths");
			writer.WriteString(pDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("RoadDeaths");
			writer.WriteString(rDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("SuicideDeaths");
			writer.WriteString(sDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("KDRatio");
			writer.WriteString(kdRatio);
			writer.WriteEndElement();
		}
		public static void createPVENode(string tDeaths, string zDeaths, string vDeaths, string fDeaths, string wDeaths,
			string iDeaths, string bDeaths, XmlTextWriter writer)
		{
			writer.WriteStartElement("TotalDeaths");
			writer.WriteString(tDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("ZombieDeaths");
			writer.WriteString(zDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("VehicleDeaths");
			writer.WriteString(vDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("FoodDeaths");
			writer.WriteString(fDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("WaterDeaths");
			writer.WriteString(wDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("InfectionDeaths");
			writer.WriteString(iDeaths);
			writer.WriteEndElement();
			writer.WriteStartElement("BleedingDeaths");
			writer.WriteString(bDeaths);
			writer.WriteEndElement();
		}
		public static void readPVPNode(UnturnedPlayer target)
		{
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string filePath = currentPath + "\\Plugins\\StatKeeper\\Stats\\";
			string targetStatFile = filePath + target.CSteamID + ".xml";

			XmlDocument targetDoc = new XmlDocument();
			targetDoc.Load(targetStatFile);
			XmlElement targetPvPRoot = targetDoc.DocumentElement;
			XmlNodeList targetPvPNodes = targetPvPRoot.SelectNodes("/Stats/PvP");

			foreach (XmlNode node in targetPvPNodes)
			{
				int tKills = int.Parse(node["TotalKills"].InnerText);
				int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
				int kdRatio = int.Parse(node["KDRatio"].InnerText);
				int gKills = int.Parse(node["GunKills"].InnerText);
				int mKills = int.Parse(node["MeleeKills"].InnerText);
				int pKills = int.Parse(node["PunchKills"].InnerText);
				int rKills = int.Parse(node["RoadKills"].InnerText);
				int gDeaths = int.Parse(node["GunDeaths"].InnerText);
				int mDeaths = int.Parse(node["MeleeDeaths"].InnerText);
				int pDeaths = int.Parse(node["PunchDeaths"].InnerText);
				int rDeaths = int.Parse(node["RoadDeaths"].InnerText);
				int sDeaths = int.Parse(node["SuicideDeaths"].InnerText);

				Rocket.Unturned.Chat.UnturnedChat.Say (target, target.CharacterName + "'s PvP Stats: Total Kills [" + tKills + "]"
					+ ", Total Deaths [" + tDeaths + "]" + ", KD [" + kdRatio + "]" + ", Gun Kills [" + gKills + "]"
					+ ", Melee Kills [" + mKills + "]" + ", Punch Kills [" + pKills + "]" + ", Road Kills [" + rKills + "]"
					+ ", Gun Deaths [" + gDeaths + "]" + ", Melee Deaths [" + mDeaths + "]" + ", Punch Deaths [" + pDeaths + "]"
					+ ", Road Deaths [" + rDeaths + "]" + ", Suicides [" + sDeaths + "]", Color.yellow);
			}
		}
		public static void readPVENode(UnturnedPlayer target)
		{
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string filePath = currentPath + "\\Plugins\\StatKeeper\\Stats\\";
			string targetStatFile = filePath + target.CSteamID + ".xml";

			XmlDocument targetDoc = new XmlDocument();
			targetDoc.Load(targetStatFile);
			XmlElement targetPvERoot = targetDoc.DocumentElement;
			XmlNodeList targetPvENodes = targetPvERoot.SelectNodes("/Stats/PvE");

			foreach (XmlNode node in targetPvENodes)
			{
				int tDeaths = int.Parse(node["TotalDeaths"].InnerText);
				int zDeaths = int.Parse(node["ZombieDeaths"].InnerText);
				int vDeaths = int.Parse(node["VehicleDeaths"].InnerText);
				int fDeaths = int.Parse(node["FoodDeaths"].InnerText);
				int wDeaths = int.Parse(node["WaterDeaths"].InnerText);
				int iDeaths = int.Parse(node["InfectionDeaths"].InnerText);
				int bDeaths = int.Parse(node["BleedingDeaths"].InnerText);

				Rocket.Unturned.Chat.UnturnedChat.Say (target, target.CharacterName + "'s PvE Stats: Total Deaths [" + tDeaths + "]" 
					+ ", Zombie Deaths [" + zDeaths + "]" + ", Vehicle Deaths [" + vDeaths + "]" + ", Food Deaths [" + fDeaths + "]"
					+ ", Water Deaths [" + wDeaths + "]" + ", Infection Deaths [" + iDeaths + "]" + ", Bleeding Deaths [" + bDeaths + "]", Color.yellow);
			}
		}
		public static void readTKNode(UnturnedPlayer target)
		{
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string filePath = currentPath + "\\Plugins\\StatKeeper\\Stats\\";
			string targetStatFile = filePath + target.CSteamID + ".xml";

			XmlDocument targetDoc = new XmlDocument();
			targetDoc.Load(targetStatFile);
			XmlElement targetPvPRoot = targetDoc.DocumentElement;
			XmlNodeList targetPvPNodes = targetPvPRoot.SelectNodes("/Stats/PvP");

			foreach (XmlNode node in targetPvPNodes)
			{
				int tKills = int.Parse(node["TotalKills"].InnerText);

				Rocket.Unturned.Chat.UnturnedChat.Say (target, target.CharacterName + "'s Total Kills: [" + tKills + "]", Color.yellow);
			}
		}
		public static void readTDNode(UnturnedPlayer target)
		{
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string filePath = currentPath + "\\Plugins\\StatKeeper\\Stats\\";
			string targetStatFile = filePath + target.CSteamID + ".xml";

			XmlDocument targetDoc = new XmlDocument();
			targetDoc.Load(targetStatFile);
			XmlElement targetPvPRoot = targetDoc.DocumentElement;
			XmlNodeList targetPvPNodes = targetPvPRoot.SelectNodes("/Stats/PvP");

			foreach (XmlNode node in targetPvPNodes)
			{
				int tDeathsPVP = int.Parse(node["TotalDeaths"].InnerText);
				Rocket.Unturned.Chat.UnturnedChat.Say (target, target.CharacterName + "'s Total Deaths: PvP[" + tDeathsPVP + "]", Color.yellow);
			}
		}
		public static void readKDNode(UnturnedPlayer target)
		{
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			string filePath = currentPath + "\\Plugins\\StatKeeper\\Stats\\";
			string targetStatFile = filePath + target.CSteamID + ".xml";

			XmlDocument targetDoc = new XmlDocument();
			targetDoc.Load(targetStatFile);
			XmlElement targetPvPRoot = targetDoc.DocumentElement;
			XmlNodeList targetPvPNodes = targetPvPRoot.SelectNodes("/Stats/PvP");

			foreach (XmlNode node in targetPvPNodes)
			{
				int kdRatio = int.Parse(node["KDRatio"].InnerText);

				Rocket.Unturned.Chat.UnturnedChat.Say (target, target.CharacterName + "'s KD: [" + kdRatio+ "]", Color.yellow);
			}
		}
	}
}

