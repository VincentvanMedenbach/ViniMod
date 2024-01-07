using System.Collections;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ViniMod.Patches
{

    [HarmonyPatch(typeof(HoarderBugAI))]
    internal class HoardingBugPatch : NetworkBehaviour
    {

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void startPatch()
        {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ViniMod-ClientExplodeRpc", ExplodeYipeeClientRpc);
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ViniMod-ServerExplodeRpc", ExplodeYipeeServerRpc);
        }

        public static HoardingBugPatch hoardingBugPatchInst = null;
        [HarmonyPatch((typeof(HoarderBugAI)), "DetectAndLookAtPlayers")]
        [HarmonyPostfix]
        public static void Postfix(HoarderBugAI __instance, ref bool ___isEnemyDead, ref float ___annoyanceMeter, ref Vector3 ___serverPosition)
        {
            PlayerControllerB closestPlayer;
            try { closestPlayer = __instance.GetClosestPlayer(true, true, true); }
            catch { closestPlayer = null; }
            if (closestPlayer == null) { return; }

            if (hoardingBugPatchInst == null)
            {
                hoardingBugPatchInst = new HoardingBugPatch();
            }
            if (closestPlayer.playerSteamId.Equals(76561198155116843))
            {
              
                if ((!___isEnemyDead && (__instance?.angryAtPlayer != null || ___annoyanceMeter > 1.5f) && Vector3.Distance(closestPlayer.serverPlayerPosition, ___serverPosition) < 10f))
                {

                    ViniModBase.mls.LogDebug("Yipeee BOOM!" + closestPlayer.transform.name + "  " + closestPlayer.playerUsername + "\n");

                    if (closestPlayer != null && !closestPlayer.isPlayerDead && !(closestPlayer != GameNetworkManager.Instance.localPlayerController))
                    {
                        hoardingBugPatchInst.TriggerMineOnLocalClientByExiting(___serverPosition);
                    }
                }
            }


        }
        private void TriggerMineOnLocalClientByExiting(Vector3 location)
        {
            Landmine.SpawnExplosion(location, true, 5f, 5f);
            ViniModBase.mls.LogDebug("Boom at " + location);
            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager == null || !networkManager.IsListening)
            {
                ViniModBase.mls.LogDebug("Server is dead?");
                return;
            }
            ViniModBase.mls.LogInfo(__rpc_exec_stage);
            if ((networkManager.IsServer || networkManager.IsHost))
            {
                ViniModBase.mls.LogDebug("Ís server, sending to clients...");

                var writer = new FastBufferWriter(sizeof(int) * 3, Allocator.Temp);
                writer.WriteValueSafe(location);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll("ViniMod-ClientExplodeRpc", writer);

            }
            else
            {
                ViniModBase.mls.LogDebug("is client, sending to Server...");
                var writer = new FastBufferWriter(sizeof(int) * 3, Allocator.Temp);
                writer.WriteValueSafe(location);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ViniMod-ServerExplodeRpc", NetworkManager.ServerClientId, writer);

            }

        }
        public static void ExplodeYipeeClientRpc(ulong clientId, FastBufferReader reader) // 1241251521U
        {
            Vector3 location;
            reader.ReadValue(out location);
            Landmine.SpawnExplosion(location, true, 5f, 5f);
            ViniModBase.mls.LogDebug("ExplodeClientRPC");
        }

        public static void ExplodeYipeeServerRpc(ulong clientId, FastBufferReader reader) //2515251523U
        {
            Vector3 location;
            reader.ReadValue(out location);
            Landmine.SpawnExplosion(location, true, 5f, 5f);
            ViniModBase.mls.LogDebug("Explode Server RPC");


            var writer = new FastBufferWriter(sizeof(int) * 3, Allocator.Temp);
            writer.WriteValueSafe(location);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll("ViniMod-ClientExplodeRpc", writer);

        }

    }
}
