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
                UISystem.Instance.ShowMessage($"Wave {currentWave}", null, 3f);

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

                if (waveTimer >= 30f)
                {
                    GameObject turnInZoneInstance = GameObject.FindGameObjectWithTag("TurnInZone");
                    if (turnInZoneInstance != null)
                    {
                        ReadFood readFood = turnInZoneInstance.GetComponent<ReadFood>();
                        if (readFood.score >= 100)
                        {
                            currentWave++;
                            UISystem.Instance.ShowMessage($"Wave {currentWave}",null,3f);
                            waveTimer = 0f;
                        }
                        else
                        {
                            UISystem.Instance.ShowMessage("Game Over");
                        }
                    }
                }

                yield return null;
            }
        }

    }
}