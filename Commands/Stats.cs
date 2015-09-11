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
using draw = System.Drawing;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace StatKeeperCommands
{
	public class StatsCommand : IRocketCommand
	{
		public bool AllowFromConsole
		{
			get
			{
				return false;
			}
		}
		public List<string> Permissions
		{
			get
			{
				return new List<string>() { 
					"statkeeper.stats"
				};
			}
		}
		public bool RunFromConsole
		{
			get { return false; }
		}

		public string Name
		{
			get { return "stats"; }
		}
		public string Syntax
		{
			get
			{
				return "<mode/stat node> <player>";
			}
		}
		public string Help
		{
			get { return "Enter player name to see stats of player or no player name to view your stats."; }
		}
		public List<string> Aliases
		{
			get { return new List<string> { "stats" }; }
		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer cPlayer = (UnturnedPlayer)caller;

			if (command.Length < 1)
			{
				return;
			}
			if (command[0].ToString().ToLower() == "pvp")
			{
				if (command.Length == 1)
				{
					UnturnedPlayer target = (UnturnedPlayer)caller;
					Factions.Utility.readPVPNode(target);
				}
				if (command.Length == 2)
				{
					UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
					if (target == null)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Player does not exist or is offline!", Color.yellow);
					}
					else
					{
						Factions.Utility.readPVPNode(target);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "pve")
			{
				if (command.Length == 1)
				{
					UnturnedPlayer target = (UnturnedPlayer)caller;
					Factions.Utility.readPVENode(target);
				}
				if (command.Length == 2)
				{
					UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
					if (target == null)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Player does not exist or is offline!", Color.yellow);
					}
					else
					{
						Factions.Utility.readPVENode(target);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "tk")
			{
				if (command.Length == 1)
				{
					UnturnedPlayer target = (UnturnedPlayer)caller;
					Factions.Utility.readTKNode(target);
				}
				if (command.Length == 2)
				{
					UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
					if (target == null)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Player does not exist or is offline!", Color.yellow);
					}
					else
					{
						Factions.Utility.readTKNode(target);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "td")
			{
				if (command.Length == 1)
				{
					UnturnedPlayer target = (UnturnedPlayer)caller;
					Factions.Utility.readTDNode(target);
				}
				if (command.Length == 2)
				{
					UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
					if (target == null)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Player does not exist or is offline!", Color.yellow);
					}
					else
					{
						Factions.Utility.readTDNode(target);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "kd")
			{
				if (command.Length == 1)
				{
					UnturnedPlayer target = (UnturnedPlayer)caller;
					Factions.Utility.readKDNode(target);
				}
				if (command.Length == 2)
				{
					UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
					if (target == null)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Player does not exist or is offline!", Color.yellow);
					}
					else
					{
						Factions.Utility.readKDNode(target);
					}
				}
			}
			else
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Not a valid mode or stat node! Select mode: pvp or pve, or a stat node: tk, td, or kd.", Color.yellow);
			}
		}
	}
}