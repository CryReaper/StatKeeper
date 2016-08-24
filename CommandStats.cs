using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;
using UnityEngine;

namespace StatKeeper
{
	public class CommandStats : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
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

        private string showPVP(UnturnedPlayer player,Stats stats)
        {
            return player.CharacterName + "'s PvP Stats: Total Kills [" + stats.TotalKills + "]"
            + ", Total Deaths [" + stats.TotalDeaths + "]" + ", KD [" + stats.KDRatio + "]" + ", Gun Kills [" + stats.GetKills(SDG.Unturned.EDeathCause.GUN.ToString()) + "]"
            + ", Melee Kills [" + stats.GetKills(SDG.Unturned.EDeathCause.MELEE.ToString()) + "]" + ", Punch Kills [" + stats.GetKills(SDG.Unturned.EDeathCause.PUNCH.ToString()) + "]" + ", Road Kills [" + stats.GetKills(SDG.Unturned.EDeathCause.ROADKILL.ToString()) + "]"
            + ", Gun Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.GUN.ToString()) + "]" + ", Melee Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.MELEE.ToString()) + "]" + ", Punch Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.PUNCH.ToString()) + "]"
            + ", Road Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.ROADKILL.ToString()) + "]" + ", Suicides [" + stats.GetDeaths(SDG.Unturned.EDeathCause.SUICIDE.ToString()) + "]";
        }

        private string showPVE(UnturnedPlayer player, Stats stats)
        {
            return player.CharacterName + "'s PvE Stats: Total Deaths [" + stats.TotalDeaths + "]"
                   + ", Zombie Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.ZOMBIE.ToString()) + "]" + ", Vehicle Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.VEHICLE.ToString()) + "]" + ", Food Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.FOOD.ToString()) + "]"
                   + ", Water Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.WATER.ToString()) + "]" + ", Infection Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.INFECTION.ToString()) + "]" + ", Bleeding Deaths [" + stats.GetDeaths(SDG.Unturned.EDeathCause.BLEEDING.ToString()) + "]";
        }

        private string showTK(UnturnedPlayer player, Stats stats)
        {
            return player.CharacterName + "'s Total Kills: [" + stats.TotalKills + "]";
        }

        private string showTD(UnturnedPlayer player, Stats stats)
        {
            return player.CharacterName + "'s Total Deaths: PvP[" + stats.TotalDeaths + "]";
        }

        private string showKD(UnturnedPlayer player, Stats stats)
        {
            return player.CharacterName + "'s KD: [" + stats.KDRatio + "]";
        }
    


        public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;

            Stats stats = player.GetComponent<StatsPlayerComponent>().Stats.Instance;

			if (command.Length < 1)
			{
				return;
			}
			if (command[0].ToString().ToLower() == "pvp")
			{
				if (command.Length == 1)
				{
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, showPVP(player, stats), Color.yellow);
                }
				if (command.Length == 2)
				{
					if(player.HasPermission("stats.admin"))
					{
						UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
                        if (target == null)
						{
							Rocket.Unturned.Chat.UnturnedChat.Say (player, "Player does not exist or is offline!", Color.yellow);
						}
						else
                        {
                            Stats targetStats = target.GetComponent<StatsPlayerComponent>().Stats.Instance;
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, showPVP(target, targetStats), Color.yellow);
                        }
                    }
					else
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (player, "You do not have permission to use this command!", Color.red);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "pve")
			{
				if (command.Length == 1)
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, showPVE(player, stats), Color.yellow);
                }
				if (command.Length == 2)
				{
					if(player.HasPermission("stats.admin"))
					{
						UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
                        if (target == null)
						{
							Rocket.Unturned.Chat.UnturnedChat.Say (player, "Player does not exist or is offline!", Color.yellow);
						}
						else
                        {
                            Stats targetStats = target.GetComponent<StatsPlayerComponent>().Stats.Instance;
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, showPVE(target, targetStats), Color.yellow);
                        }
                    }
					else
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (player, "You do not have permission to use this command!", Color.red);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "tk")
			{
				if (command.Length == 1)
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, showTK(player, stats), Color.yellow);
                }
				else if (command.Length == 2)
				{
					if(player.HasPermission("stats.admin"))
					{
						UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
                        if (target == null)
						{
							Rocket.Unturned.Chat.UnturnedChat.Say (player, "Player does not exist or is offline!", Color.yellow);
						}
						else
                        {
                            Stats targetStats = target.GetComponent<StatsPlayerComponent>().Stats.Instance;
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, showTK(target, targetStats), Color.yellow);
                        }
					}
					else
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (player, "You do not have permission to use this command!", Color.red);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "td")
			{
				if (command.Length == 1)
				{
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, showTD(player, stats), Color.yellow);
                }
				else if (command.Length == 2)
				{
					if(player.HasPermission("stats.admin"))
                    {
                        UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
                        if (target == null)
						{
							Rocket.Unturned.Chat.UnturnedChat.Say (player, "Player does not exist or is offline!", Color.yellow);
						}
						else
                        {
                            Stats targetStats = target.GetComponent<StatsPlayerComponent>().Stats.Instance;
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, showTD(target, targetStats), Color.yellow);
                        }
					}
					else
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (player, "You do not have permission to use this command!", Color.red);
					}
				}
			}
			else if (command[0].ToString().ToLower() == "kd")
			{
				if (command.Length == 1)
                {
                    Rocket.Unturned.Chat.UnturnedChat.Say(player, showKD(player, stats), Color.yellow);
                }
				else if (command.Length == 2)
				{
					if(player.HasPermission("stats.admin"))
                    {
                        UnturnedPlayer target = UnturnedPlayer.FromName(command[1].ToString().ToLower());
                        if (target == null)
						{
							Rocket.Unturned.Chat.UnturnedChat.Say (player, "Player does not exist or is offline!", Color.yellow);
						}
						else
                        {
                            Stats targetStats = target.GetComponent<StatsPlayerComponent>().Stats.Instance;
                            Rocket.Unturned.Chat.UnturnedChat.Say(player, showKD(target, targetStats), Color.yellow);
                        }
					}
					else
					{
						Rocket.Unturned.Chat.UnturnedChat.Say (player, "You do not have permission to use this command!", Color.red);
					}
				}
			}
			else
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (player, "Not a valid mode or stat node! Select mode: pvp or pve, or a stat node: tk, td, or kd.", Color.yellow);
			}
		}
	}
}