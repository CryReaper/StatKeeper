using Rocket.Core.Assets;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.IO;

namespace StatKeeper
{
    public class StatsPlayerComponent : UnturnedPlayerComponent
    {
        public XMLFileAsset<Stats> Stats;
        public void Start()
        {
            Stats = new XMLFileAsset<StatKeeper.Stats>(Path.Combine(Plugin.Instance.Directory,"Stats/"+Player.Id+".xml"));
            Stats.Instance.SteamID = (ulong)Player.CSteamID;
            Player.Events.OnDeath += (UnturnedPlayer player, SDG.Unturned.EDeathCause cause, SDG.Unturned.ELimb limb, Steamworks.CSteamID m) =>
            {
                UnturnedPlayer murderer = UnturnedPlayer.FromCSteamID(m);
                if (murderer != null && PlayerTool.getSteamPlayer(m) != null)
                {
                    XMLFileAsset<Stats> killerStats = murderer.GetComponent<StatsPlayerComponent>().Stats;
                    
                    killerStats.Instance.Kills[cause.ToString()] = killerStats.Instance.GetKills(cause.ToString()) + 1;
                    killerStats.Instance.RecalculateStats();
                    killerStats.Save();
                }
                Stats.Instance.Deaths[cause.ToString()] = Stats.Instance.GetDeaths(cause.ToString()) + 1;
                Stats.Instance.RecalculateStats();
                Stats.Save();
            };
            Stats.Instance.RecalculateStats();
        }

        public void OnDisable()
        {
            Stats.Instance.RecalculateStats();
            Stats.Save();
        }
    }
}
