using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatKeeper
{
    public class Stats : IDefaultable
    {
        public int GetKills(SDG.Unturned.EDeathCause cause)
        {
            if (!Kills.ContainsKey(cause)) return 0;
            return Kills[cause];
        }

        public int GetDeaths(SDG.Unturned.EDeathCause cause)
        {
            if (!Deaths.ContainsKey(cause)) return 0;
            return Deaths[cause];
        }

        public ulong SteamID { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public DateTime Created { get; private set; } = DateTime.Now;

        public Dictionary<SDG.Unturned.EDeathCause, int> Kills { get; private set; } = new Dictionary<SDG.Unturned.EDeathCause, int>();
        public Dictionary<SDG.Unturned.EDeathCause, int> Deaths { get; private set; } = new Dictionary<SDG.Unturned.EDeathCause, int>();

        public double KDRatio { get; set; } = 0;
        public int TotalKills { get; set; } = 0;
        public int TotalDeaths { get; set; } = 0;

        public void RecalculateStats()
        {
            TotalDeaths = Deaths.Values.Sum();
            TotalKills = Kills.Values.Sum();
            KDRatio =  TotalKills / TotalDeaths;
            LastUpdated = DateTime.Now;
        }

        public Stats() { }

        public void LoadDefaults()
        {
            SteamID = 0;
            LastUpdated = DateTime.Now;
            Created = DateTime.Now;
            Kills = new Dictionary<SDG.Unturned.EDeathCause, int>();
            Deaths = new Dictionary<SDG.Unturned.EDeathCause, int>();
            KDRatio = 0;
            TotalKills = 0;
            TotalDeaths = 0;
        }
    }
}
