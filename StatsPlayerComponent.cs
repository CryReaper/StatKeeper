using Rocket.Core.Assets;
using Rocket.Unturned.Player;
using Steamworks;
using System.IO;

namespace StatKeeper
{
    public class StatsPlayerComponent : UnturnedPlayerComponent
    {
        public XMLFileAsset<Stats> Stats;
        public void Awake()
        {
            Stats =  new XMLFileAsset<StatKeeper.Stats>(Path.Combine(Plugin.Instance.Directory,"Stats/"+Player.Id+".xml"));
            Stats.Instance.SteamID = (ulong)Player.CSteamID;
            Player.Events.OnDeath += OnDeath;
            Stats.Instance.RecalculateStats();
        }

        public void OnDisable()
        {
            Stats.Instance.RecalculateStats();
            Stats.Save();
        }
        
        private void OnDeath(UnturnedPlayer player, SDG.Unturned.EDeathCause cause, SDG.Unturned.ELimb limb, Steamworks.CSteamID murderer)
        {
            if (murderer != (CSteamID)0)
            {
                XMLFileAsset<Stats> killerStats = UnturnedPlayer.FromCSteamID(murderer).GetComponent<StatsPlayerComponent>().Stats;
                killerStats.Instance.Kills[cause]++;
                killerStats.Instance.RecalculateStats();
                killerStats.Save();
            }
            Stats.Instance.Deaths[cause]++;
            Stats.Instance.RecalculateStats();
            Stats.Save();
        }
    }
}
