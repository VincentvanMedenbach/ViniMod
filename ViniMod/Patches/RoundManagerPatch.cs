using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace ViniMod.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {


        [HarmonyPatch((typeof(RoundManager)), "LoadNewLevel")]
        [HarmonyPrefix]
        public static void spawnYippees( ref SelectableLevel newLevel)
        {


            //if (!RoundManager.Instance.IsServer)
            //{
            //    return;
            //}
            ViniModBase.mls.LogDebug(newLevel.PlanetName);
            if (newLevel.PlanetName == "71 Gordion")
            {
                bool gotEnemy = false;
                bool addedEnemy = false;

                foreach (var item in newLevel.OutsideEnemies)
                {
                    if (item.enemyType.enemyPrefab.GetComponent<NutcrackerEnemyAI>() != null)
                    {
                        gotEnemy = true;
                    }
                }
                if (!gotEnemy)
                {
                    foreach (var level in StartOfRound.Instance.levels)
                    {
                        foreach (var enemy in level.Enemies)
                        {
                            if (enemy.enemyType.enemyPrefab.GetComponent<NutcrackerEnemyAI>() != null)
                            {
                                if (!addedEnemy)
                                {
                                    addedEnemy = true;
                                    newLevel.OutsideEnemies.Add(enemy);
                                }
                            }
                        }
                    }

                }
                for (int i = 0; i < newLevel.OutsideEnemies.Count; i++)
                {
                    if (newLevel.OutsideEnemies[i].enemyType.enemyPrefab.GetComponent<NutcrackerEnemyAI>() != null)
                    {

                        Vector3 position = RoundManager.Instance.GetNavMeshPosition(new Vector3(10, -2.67f, 10));

                        GameObject obj = UnityEngine.Object.Instantiate(newLevel.OutsideEnemies[i].enemyType.enemyPrefab, position, Quaternion.Euler(Vector3.zero));
                        obj.gameObject.GetComponentInChildren<NetworkObject>().Spawn(destroyWithScene: true);
                        RoundManager.Instance.SpawnedEnemies.Add(obj.GetComponent<EnemyAI>());
                        obj.GetComponent<EnemyAI>().enemyType.numberSpawned++;
                        obj.GetComponent<NavMeshAgent>().enabled = true;
                    }
                    else
                    {
                        ViniModBase.mls.LogDebug("Found a " + newLevel.OutsideEnemies[i].enemyType.name + ":(");

                    }
                }
            }
            //From here spawning ONLY 1 mob
      
            //End test code 
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
            //}
            //newLevel.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0, 500f));
            //ViniModBase.mls.LogInfo("Spawning yipeeees!");
            //GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("OutsideAINode");
            //HoarderBugAI yippee = null; ;
            //foreach (var item in newLevel.Enemies)
            //{
            //    item.enemyType.probabilityCurve = new AnimationCurve(new Keyframe(0, 1000));
            //}
            //bool gotEnemy = false;
            //bool addedEnemy = false;
            //foreach (var item in newLevel.Enemies)
            //{
            //    if (item.enemyType.enemyPrefab.GetComponent<HoarderBugAI>() != null)
            //    {
            //        gotEnemy = true;
            //    }
            //}
            //if (!gotEnemy)
            //{
            //    foreach (var level in StartOfRound.Instance.levels)
            //    {
            //        foreach (var enemy in level.Enemies)
            //        {
            //            if (enemy.enemyType.enemyPrefab.GetComponent<HoarderBugAI>() != null)
            //            {
            //                if (!addedEnemy)
            //                {
            //                    addedEnemy = true;
            //                    newLevel.Enemies.Add(enemy);
            //                }
            //            }
            //        }
            //    }

            //}
            //for (int i = 0; i < newLevel.Enemies.Count; i++)
            //{
            //    newLevel.maxEnemyPowerCount = 1000;
            //    newLevel.Enemies[i].rarity = 0;
            //    if (newLevel.Enemies[i].enemyType.enemyPrefab.GetComponent<HoarderBugAI>() != null)
            //    {
            //        newLevel.Enemies[i].rarity = 999;
            //        yippee = newLevel.Enemies[i].enemyType.enemyPrefab.GetComponent<HoarderBugAI>();
            //        newLevel.Enemies[i].enemyType.MaxCount = 40;
            //        ViniModBase.mls.LogDebug("Found a " + yippee.name);
            //    }
            //    else
            //    {
            //        ViniModBase.mls.LogDebug("Found a "+ newLevel.Enemies[i].enemyType.name + ":(");

            //    }
        }
        //[HarmonyPatch((typeof(RoundManager)), "LoadNewLevel")]
        //[HarmonyPrefix]
        //public static void spawnTestMobs( ref SelectableLevel newLevel)
        //{
        //    bool gotEnemy = false;
        //    bool addedEnemy = false;
        //    EnemyAI yippee = null;
        //    foreach (var item in newLevel.Enemies)
        //    {
        //        item.enemyType.probabilityCurve = new AnimationCurve(new Keyframe(0, 1000));
        //    }
        //    foreach (var item in newLevel.Enemies)
        //    {
        //        if (item.enemyType.enemyPrefab.GetComponent<SpringManAI>() != null)
        //        {
        //            gotEnemy = true;
        //        }
        //    }
        //    if (!gotEnemy)
        //    {
        //        foreach (var level in StartOfRound.Instance.levels)
        //        {
        //            foreach (var enemy in level.Enemies)
        //            {
        //                if (enemy.enemyType.enemyPrefab.GetComponent<SpringManAI>() != null)
        //                {
        //                    if (!addedEnemy)
        //                    {
        //                        addedEnemy = true;
        //                        newLevel.Enemies.Add(enemy);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    for (int i = 0; i < newLevel.Enemies.Count; i++)
        //    {
        //        newLevel.maxEnemyPowerCount = 1000;
        //        newLevel.Enemies[i].rarity = 0;
        //        if (newLevel.Enemies[i].enemyType.enemyPrefab.GetComponent<SpringManAI>() != null)
        //        {
        //            newLevel.Enemies[i].rarity = 999;
        //            yippee = newLevel.Enemies[i].enemyType.enemyPrefab.GetComponent<SpringManAI>();
        //            newLevel.Enemies[i].enemyType.MaxCount = 40;
        //            ViniModBase.mls.LogDebug("Found a " + yippee.name);
        //        }
        //        else
        //        {
        //            ViniModBase.mls.LogDebug("Found a " + newLevel.Enemies[i].enemyType.name + ":(");

        //        }
        //    }
        //}
    }
}



