using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViniMod
{
    public class ConfigSettings
    {
        public ConfigEntry<string> SteamIds { get; set; }
        public List<ulong> SteamIdsList = new List<ulong>();
        public ConfigEntry<string> CoilHeadTargets { get; set; }
        public List<ulong> CoilHeadTargetsList = new List<ulong>();
        public ConfigEntry<string> YippeBoomTargets { get; set; }

        public List<ulong> YippeBoomTargetsList = new List<ulong>();
        public ConfigEntry<string> YippeDanceTargets { get; set; }
        public ConfigEntry<bool> ApplyToAll { get; set; }



        public void LoadConfigs()
        {
            SteamIds = ViniModBase.Instance.Config.Bind<string>("SteamIds", "SteamIdsList", "0", "Add steamID64 (Dec) in the following format: 124551,12414124,5125251");
            CoilHeadTargets = ViniModBase.Instance.Config.Bind<string>("CoilHeadTargets", "CoilHeadTargetsList", "0", "Add steamID64 (Dec) in the following format: 124551,12414124,5125251");
            YippeBoomTargets = ViniModBase.Instance.Config.Bind<string>("YippeBoomTargets", "YippeBoomTargetsList", "0", "Add steamID64 (Dec) in the following format: 124551,12414124,5125251");


            ApplyToAll = ViniModBase.Instance.Config.Bind<bool>("ApplyToAll", "ApplyToAll", true, "Apply trolling to all players in game, This will ignore the steamIDS");

            try
            { 
                SteamIdsList = SteamIds.Value.Split(',').Select(ulong.Parse).ToList();
                CoilHeadTargetsList = CoilHeadTargets.Value.Split(',').Select(ulong.Parse).ToList();
                YippeBoomTargetsList = YippeBoomTargets.Value.Split(',').Select(ulong.Parse).ToList();
            }
            catch (Exception e) { ViniModBase.mls.LogError("Invalid format for steamIds!" + e.Message); }

        }
    }
}