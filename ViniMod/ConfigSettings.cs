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

        public void LoadConfigs()
        {
            SteamIds = ViniModBase.Instance.Config.Bind<string>("SteamIds", "SteamIdsList", "76561198352066512,76561198260711933", "Add steamID64 (Dec) in the following format: 124551,12414124,5125251");
            try { 
                SteamIdsList = SteamIds.Value.Split(',').Select(ulong.Parse).ToList(); 
            }
            catch (Exception e) { ViniModBase.mls.LogError("Invalid format for steamIds!" + e.Message); }

        }
    }
}