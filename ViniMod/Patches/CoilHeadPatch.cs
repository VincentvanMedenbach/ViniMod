using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ViniMod.Patches
{
    internal class CoilHeadPatch
    {
        [HarmonyPatch((typeof(SpringManAI)), "Update")]
        [HarmonyPostfix]
        public static void Prefix(SpringManAI __instance, ref float ___currentAnimSpeed)
        {
            List<PlayerControllerB> players = StartOfRound.Instance.allPlayerScripts.Where(item => ViniModBase.configSettings.CoilHeadTargetsList.Contains(item.playerSteamId)).ToList();
            if (ViniModBase.configSettings.ApplyToAll.Value)
            {
                players = StartOfRound.Instance.allPlayerScripts.ToList();
            }

            foreach (PlayerControllerB item in players)
            {

                if (item.HasLineOfSightToPosition(__instance.transform.position + Vector3.up * 1.6f, 68f) && Vector3.Distance(item.gameplayCamera.transform.position, __instance.eye.position) > 0.3f)
                {
                    if (!__instance.IsOwner)
                    {
                        __instance.ChangeOwnershipOfEnemy(GameNetworkManager.Instance.localPlayerController.actualClientId);
                    }

                    ViniModBase.mls.LogDebug("Active!");
                    __instance.SetAnimationGoServerRpc();
                    ___currentAnimSpeed = 0.01f;
                    __instance.creatureAnimator.SetFloat("walkSpeed", 0.01f);
                    if (__instance.IsOwner)
                    {
                        __instance.agent.speed = 0.01f;
                    }

                }
            }
        }

    }
}

