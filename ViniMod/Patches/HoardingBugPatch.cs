using System.Collections;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ViniMod.Patches
{

    [HarmonyPatch(typeof(HoarderBugAI))]
    internal class HoardingBugPatch :NetworkBehaviour
    {

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void startPatch() {
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ViniMod-ClientExplodeRpc", ExplodeYipeeClientRpc);
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ViniMod-ServerExplodeRpc", ExplodeYipeeServerRpc);
        }

        public static HoardingBugPatch hoardingBugPatchInst = null;
        [HarmonyPatch("DetectAndLookAtPlayers")]
        [HarmonyPostfix]
        public static void Postfix(ref PlayerControllerB ___watchingPlayer, ref bool ___isEnemyDead, ref float ___annoyanceMeter, ref Vector3 ___serverPosition)
        {
            if(___watchingPlayer != null) { 
            ViniModBase.mls.LogDebug(___watchingPlayer.playerSteamId);
            if (hoardingBugPatchInst == null)
            {
                hoardingBugPatchInst = new HoardingBugPatch();
            }
            if (!___isEnemyDead && ___annoyanceMeter > 1.5f || ___watchingPlayer.playerSteamId.Equals("STEAM_0:0:119460879")) { 
            ViniModBase.mls.LogDebug("Yipeee BOOM!" + ___watchingPlayer.transform.name + "  "+ ___watchingPlayer.name + "\n" );
           
            

            if (___watchingPlayer != null && !___watchingPlayer.isPlayerDead && !(___watchingPlayer != GameNetworkManager.Instance.localPlayerController))
            {  
                hoardingBugPatchInst.TriggerMineOnLocalClientByExiting(___serverPosition);
            }
            }}

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
