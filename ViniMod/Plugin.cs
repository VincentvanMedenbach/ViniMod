using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using ViniMod.Patches;

namespace ViniMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class ViniModBase : BaseUnityPlugin
    {
        private const string modGUID = "ViniMod";
        private const string modName = "Vini MOD";
        private const string modVersion = "1.0.0";
        public static ConfigSettings configSettings = new ConfigSettings();
        private readonly Harmony harmony = new Harmony(modGUID);

        public static ViniModBase Instance;
        public static ManualLogSource mls;
        [RuntimeInitializeOnLoadMethod]
        internal static void InitializeRPCS_Landmine()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ViniMod-ClientExplodeRpc", HoardingBugPatch.ExplodeYipeeClientRpc);
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ViniMod-ServerExplodeRpc", HoardingBugPatch.ExplodeYipeeServerRpc);
        }
        void Awake() //Entrypoint!
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogDebug("ViniMod has awoken!");

            configSettings.LoadConfigs();
            mls.LogDebug("Config has awoken!");

            mls = Logger;
            harmony.PatchAll(typeof(ViniModBase));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(HoardingBugPatch));



        }


    }
}
