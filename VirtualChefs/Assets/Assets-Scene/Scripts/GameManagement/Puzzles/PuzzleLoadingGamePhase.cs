// Copyright (c) Meta Platforms, Inc. and affiliates.

using System.Collections;
using System.Linq;
using CrypticCabinet.Photon;
using CrypticCabinet.UI;
using Fusion;
using UnityEngine;

namespace CrypticCabinet.GameManagement.Puzzles
{
    [CreateAssetMenu(fileName = "New CrypticCabinet Game Phase", menuName = "CrypticCabinet/Sand Puzzle GamePhase")]
    public class SandPuzzleGamePhase : GamePhase
    {
        [SerializeField] private GameObject[] m_prefabSandPuzzlePrefabs;
        private float waveTimer = 0f;
        private int currentWave = 1;

        protected override void InitializeInternal()
        {
            if (m_prefabSandPuzzlePrefabs == null || m_prefabSandPuzzlePrefabs.Length <= 0)
            {
                Debug.LogError("No Prefabs specified!");
                return;
            }

            if (PhotonConnector.Instance != null && PhotonConnector.Instance.Runner != null)
            {
                _ = GameManager.Instance.StartCoroutine(HandleSpawn());
                _ = GameManager.Instance.StartCoroutine(HandleWaves());
                UISystem.Instance.ShowMessage($"Wave {currentWave}", null, 2f);
             
            }
            else
            {
                Debug.LogWarning("Couldn't instantiate sand puzzle prefab!");
            }
        }

        private IEnumerator HandleSpawn()
        {
            // Grabs each prefab from the prefabSandPuzzlePrefabs list and waits until the Spawn method returns true,
            // once it does it continues iterating over the list until all prefabs are spawned
            return m_prefabSandPuzzlePrefabs.Select(puzzlePrefab => new WaitUntil(() => Spawn(puzzlePrefab))).GetEnumerator();

        }

        private bool Spawn(GameObject instance)
        {
            var spawned = false;
            _ = PhotonConnector.Instance.Runner.Spawn(instance, onBeforeSpawned: delegate (NetworkRunner runner,
                NetworkObject o)
            {
                spawned = true;
            });
            return spawned;
        }
        private IEnumerator HandleWaves()
        {
            while (true)
            {
                GameObject timerInstance = GameObject.FindGameObjectWithTag("Timer");
                countdownTimer countdownTimer = timerInstance.GetComponent<countdownTimer>();
                
             
        
                if (currentWave == 1 && countdownTimer.gameTime <= 0f) // 3 minutes for Wave 1
                {
                    GameObject turnInZoneInstance = GameObject.FindGameObjectWithTag("TurnInZone");
                    if (turnInZoneInstance != null)
                    {
                        ReadFood readFood = turnInZoneInstance.GetComponent<ReadFood>();
                        if (readFood.score >= 100)
                        {
                            currentWave++;
                            UISystem.Instance.ShowMessage($"Wave {currentWave}", null, 2f);
                            countdownTimer.gameTime = 120f;
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage("Game Over", null, -1);
                            
                            yield break;
                        }
                    }
                }
                else if (currentWave == 2 && countdownTimer.gameTime <= 0f) // 2 minutes for Wave 2
                {
                    GameObject turnInZoneInstance = GameObject.FindGameObjectWithTag("TurnInZone");
                    if (turnInZoneInstance != null)
                    {
                        ReadFood readFood = turnInZoneInstance.GetComponent<ReadFood>();
                        if (readFood.score >= 70)
                        {
                            currentWave++;
                            UISystem.Instance.ShowMessage($"Wave {currentWave}", null, 2f);
                            countdownTimer.gameTime = 60f;
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage("Game Over", null, -1);
                            
                            yield break;
                        }
                    }
                }
                else if (currentWave == 3 && countdownTimer.gameTime <= 0f) // 1 minutes for Wave 3
                {
                    GameObject turnInZoneInstance = GameObject.FindGameObjectWithTag("TurnInZone");
                    if (turnInZoneInstance != null)
                    {
                        ReadFood readFood = turnInZoneInstance.GetComponent<ReadFood>();
                        if (readFood.score >= 50)
                        {
                            currentWave++;
                            UISystem.Instance.ShowMessage("You win!", null, -1);
                           
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage("Game Over", null, -1);
                            
                            yield break;
                        }
                    }
                }
        
                yield return null;
            }
    
}

    }
}