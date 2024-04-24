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
                waveTimer += Time.deltaTime;
                
        
                if (currentWave == 1 && waveTimer >= 300f) // 3 minutes for Wave 1
                {
                    GameObject TimerInstance = GameObject.FindGameObjectWithTag("Timer");
                    
                    GameObject ScoreInstance = GameObject.FindGameObjectWithTag("ScoreBox");

                    if (ScoreInstance != null)
                    {
                        countdownTimer CountdownTimer = TimerInstance.GetComponent<countdownTimer>();
                        TotalScoreReader totalScore = ScoreInstance.GetComponent<TotalScoreReader>();
                        
                        if (totalScore.totalScore >= 100)
                        {
                
                            currentWave++;
                            
                            UISystem.Instance.ShowMessage($" Your Score: {(int)(CountdownTimer.gameTime)}! \nYour Score: {(int)(totalScore.totalScore)}! \n Wave {currentWave}", null, 2f);
                            waveTimer = 0f;
                            CountdownTimer.gameTime = 180f;
                            totalScore.totalScore = 0;
                            
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage($"Your Score: {(int)(totalScore.totalScore)}! \n Game Over!!", null, -1);
                            
                            yield break;
                        }
                    }
                }
                else if (currentWave == 2 && waveTimer >= 180f) // 2 minutes for Wave 2
                {
                    GameObject TimerInstance = GameObject.FindGameObjectWithTag("Timer");
                    GameObject ScoreInstance = GameObject.FindGameObjectWithTag("ScoreBox");
                    if (ScoreInstance != null)
                    {
                        countdownTimer CountdownTimer = TimerInstance.GetComponent<countdownTimer>();
                        TotalScoreReader totalScore = ScoreInstance.GetComponent<TotalScoreReader>();
                        if (totalScore.totalScore >= 100)
                        {
                   
                            currentWave++;
                            UISystem.Instance.ShowMessage($"Your Score: {(int)(totalScore.totalScore)}! \n Wave {currentWave}", null, 2f);
                            CountdownTimer.gameTime = 120f;
                            waveTimer = 0f;
                            totalScore.totalScore = 0;
                         
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage($"Your Score: {(int)(totalScore.totalScore)}! \n Game Over!!", null, -1);
                            
                            yield break;
                        }
                    }
                }
                else if (currentWave == 3 && waveTimer >= 120f) // 2 minutes for Wave 3
                {
                    
                    GameObject ScoreInstance = GameObject.FindGameObjectWithTag("ScoreBox");
                    if (ScoreInstance != null)
                    {   
                        TotalScoreReader totalScore = ScoreInstance.GetComponent<TotalScoreReader>();
                        if (totalScore.totalScore >= 100)
                        {
                         
                  
                            UISystem.Instance.ShowMessage($" Your Score: {(int)(totalScore.totalScore)}! \nYou Won!", null, -1);
                      
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage($"Your Score: {(int)(totalScore.totalScore)}! \n Game Over!!", null, -1);
                            
                            yield break;
                        }
                    }
                }
        
                yield return null;
            }
    
}

    }
}