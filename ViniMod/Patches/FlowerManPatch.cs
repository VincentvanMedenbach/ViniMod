
using System.Linq;
using GameNetcodeStuff;
using HarmonyLib;


namespace ViniMod.Patches
{
    internal class FlowerManPatch
    {
        [HarmonyPatch((typeof(FlowermanAI)), "DoAIInterval")]
        [HarmonyPrefix]
        public static void Prefix(HoarderBugAI __instance, ref PlayerControllerB ___targetPlayer)
        {
            if (!___targetPlayer.isPlayerDead)
            {
                PlayerControllerB player = StartOfRound.Instance.allPlayerScripts.Where(item => ViniModBase.configSettings.SteamIdsList.Contains(item.playerSteamId)).First();
       
                ViniModBase.mls.LogDebug(player.name);
                if (player != null) { __instance.targetPlayer = player; }
            }

        }
    }
}

