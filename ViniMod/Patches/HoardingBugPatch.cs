using System.Collections;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace ViniMod.Patches
{

    [HarmonyPatch(typeof(HoarderBugAI))]
    internal class HoardingBugPatch :NetworkBehaviour
    {
        private bool hasExploded;
        private bool sendingExplosionRPC;
       

        public static HoardingBugPatch hoardingBugPatchInst = null;
        [HarmonyPatch("OnCollideWithPlayer")]
        [HarmonyPostfix]
        public static void Postfix(Collider other)
        {
            
            if (hoardingBugPatchInst == null)
            {
                hoardingBugPatchInst = new HoardingBugPatch();
            }
            ViniModBase.mls.LogInfo("Yipeee BOOM!" + other.transform.name + "  "+ other.name + "\n" );
            PlayerControllerB component = other.gameObject.GetComponent<PlayerControllerB>();
  

            if (component != null && !component.isPlayerDead && !(component != GameNetworkManager.Instance.localPlayerController))
            {
               
                hoardingBugPatchInst.TriggerMineOnLocalClientByExiting(other.transform.position);
            }
            //Vector3 explosionPosition, bool spawnExplosionEffect = false, float killRange = 1f, float damageRange = 1f
            //Landmine.SpawnExplosion(other.transform.position, true, 5, 5);

        }
        private void TriggerMineOnLocalClientByExiting(Vector3 location)
        {
            //Add hasExploded!
            Landmine.SpawnExplosion(location, true, 5f, 5f);
            ViniModBase.mls.LogInfo("Boom at " + location);
            sendingExplosionRPC = true;
            ExplodeYipeeServerRpc();

        }
        [ClientRpc]
        public void ExplodeYipeeClientRpc() // 1241251521U
        {
            ViniModBase.mls.LogInfo("ExplodeClientRPC");

            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager == null || !networkManager.IsListening)
            {
                return;
            }
            if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
            {
                ClientRpcParams clientRpcParams = default(ClientRpcParams);
                FastBufferWriter bufferWriter = __beginSendClientRpc(1241251521U, clientRpcParams, RpcDelivery.Reliable);
                __endSendClientRpc(ref bufferWriter, 1241251521U, clientRpcParams, RpcDelivery.Reliable);
                ViniModBase.mls.LogInfo("Sending to client.... SEND");

            }
            if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient || networkManager.IsHost))
            {
                if (sendingExplosionRPC)
                {
                    sendingExplosionRPC = false;
                }
                else
                {
                    ViniModBase.mls.LogInfo("Boom at " + base.transform.position);
                    Landmine.SpawnExplosion(base.transform.position, true, 5, 5);
                }
            }
        }
     
        [ServerRpc(RequireOwnership = false)]
        public void ExplodeYipeeServerRpc() //2515251523U
        {
            ViniModBase.mls.LogInfo("Explode Server RPC");

            NetworkManager networkManager = base.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                if (__rpc_exec_stage != __RpcExecStage.Server && (networkManager.IsClient || networkManager.IsHost))
                {
                    ServerRpcParams serverRpcParams = default(ServerRpcParams);
                    FastBufferWriter bufferWriter = __beginSendServerRpc(2515251523U, serverRpcParams, RpcDelivery.Reliable);
                    __endSendServerRpc(ref bufferWriter, 2515251523U, serverRpcParams, RpcDelivery.Reliable);
                    ViniModBase.mls.LogInfo("Sending to server.... SEND");

                }
                if (__rpc_exec_stage == __RpcExecStage.Server && (networkManager.IsServer || networkManager.IsHost))
                {
                    ViniModBase.mls.LogInfo("Calling sending to client RPC");

                    ExplodeYipeeClientRpc();
                }
            }

        }
        public static void __rpc_handler_1241251521U(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
        {
            ViniModBase.mls.LogInfo("Client received!?!");

            NetworkManager networkManager = target.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
                hoardingBugPatchInst.__rpc_exec_stage = __RpcExecStage.Client;
                ((HoardingBugPatch)target).ExplodeYipeeClientRpc();
                hoardingBugPatchInst.__rpc_exec_stage = __RpcExecStage.None;
            }
        }

        public static void __rpc_handler_2515251523U(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
        {
            ViniModBase.mls.LogInfo("Host received?!");

            NetworkManager networkManager = target.NetworkManager;
            if ((object)networkManager != null && networkManager.IsListening)
            {
               hoardingBugPatchInst.__rpc_exec_stage = __RpcExecStage.Server;
                ((HoardingBugPatch)target).ExplodeYipeeServerRpc();
                hoardingBugPatchInst.__rpc_exec_stage = __RpcExecStage.None;
            }
        }
    }
        }
