using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace ViniMod.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {

        
        [HarmonyPatch("LoadNewLevel")]
        [HarmonyPrefix]
        public static void spawnYippees(ref List<EnemyAI> ___SpawnedEnemies, ref SelectableLevel ___currentLevel)
        {
            ___currentLevel.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0, 500f));
            ViniModBase.mls.LogInfo("Spawning yipeeees!");
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("OutsideAINode");
            HoarderBugAI yippee = null; ;
            for (int i = 0; i < ___currentLevel.Enemies.Count; i++)
            {

                ___currentLevel.Enemies[i].rarity = 0;
                if (___currentLevel.Enemies[i].enemyType.enemyPrefab.GetComponent<HoarderBugAI>() != null)
                {
                    ___currentLevel.Enemies[i].rarity = 999;
                    yippee = ___currentLevel.Enemies[i].enemyType.enemyPrefab.GetComponent<HoarderBugAI>();
                    ___currentLevel.Enemies[i].enemyType.MaxCount = 15;
                    ViniModBase.mls.LogInfo("Found a yippee!");
                }
                else
                {
                    ViniModBase.mls.LogInfo("No Yippee FOUND :(");

                }
            }
            //if (yippee != null) {

            //     GameObject[] outsideAINodesoutsideAINodes = (from x in GameObject.FindGameObjectsWithTag("OutsideAINode")
            //                      orderby Vector3.Distance(x.transform.position, Vector3.zero)
            //                      select x).ToArray();

            //    for (int i = 0 ; i < 10 && i < outsideAINodesoutsideAINodes.Length; i++)
            //    {

            //        yippee.gameObject.GetComponentInChildren<NetworkObject>().Spawn(destroyWithScene: true);
            //        ViniModBase.mls.LogInfo("Adding Yipeee!");
            //        if (yippee != null)
            //        {
            //            ViniModBase.mls.LogInfo("Yippeeee spawned?!");

            //            ___SpawnedEnemies.Add(yippee.GetComponent<EnemyAI>());
            //            yippee.GetComponent<EnemyAI>().enemyType.numberSpawned++;
            //            continue;
            //        }
            //        break;
            //    } 
            //}
        }
    }
    }

