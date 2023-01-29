using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Managers.Waving
{
    public class WaveManager : MonoBehaviour
    {
        [Serializable]
        private class WaveClass
        {
            public GameObjectPool gameObjectPool;
            
            [Header("Wave Properties")]
            [Range(0, 10)] public int initialWaveSize;
            [Space]
            [Range(0, 10)] public int waveSizeIncreaseCount;
            [Range(0, 10)] public int increaseWaveSizeAfterWaves;
            public bool canSurviveMultipleWaves;
            [Space]
            public Vector3 spawnRange;
        }
        
        [SerializeField] List<WaveClass> waveClasses;

        [Space]
        [SerializeField] private IntObject WaveCountSO;
        [Range(1, 10)] [SerializeField] private float waveInterval = 2f;
        
        
        [Space]
        [Tooltip("A wave object is spawned in a random position, these layers lets us make sure they don't spawn in buildings or so.")]
        [SerializeField] private LayerMask collidableLayers;
        
        
        Dictionary<GameObject, WaveClass> lastSpawnedWaveDictionary = new();

        void Awake()
        {
            if (waveClasses.Count == 0)
                enabled = false;
        }

        void Start()
        {
            WaveCountSO.value = 0;

            StartCoroutine(IGenerateWaveWhenDead());
        }

        private IEnumerator IGenerateWaveWhenDead()
        {
            while (true)
            {
                if (IsCurrentWaveDead())
                {
                    yield return new WaitForSeconds(waveInterval);
                    
                    GenerateNewWave();

                    SetRandomPositions();
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        
        private bool IsCurrentWaveDead()
        {
            foreach (KeyValuePair<GameObject, WaveClass> dictionary in lastSpawnedWaveDictionary)
            {
                if (!dictionary.Value.canSurviveMultipleWaves && dictionary.Key.activeInHierarchy)
                    return false;
            }

            return true;
        }

        private void GenerateNewWave()
        {
            WaveCountSO.value++;
            
            lastSpawnedWaveDictionary.Clear();

            foreach (WaveClass currentWaveClass in waveClasses)
            {
                // We subtract 1 from the WaveCount to equalize the right side of the sum to 0
                // for the initial wave size at the 1st wave.
                int waveSize = currentWaveClass.initialWaveSize + currentWaveClass.waveSizeIncreaseCount *
                    ((WaveCountSO.value - 1) / currentWaveClass.increaseWaveSizeAfterWaves);
                
                // We do this check not to get an exception from the game object pool.
                if (waveSize > 0)
                {
                    foreach (GameObject newWaveObject in currentWaveClass.gameObjectPool.GetFromQueue(waveSize))
                    {
                        lastSpawnedWaveDictionary.Add(newWaveObject ,currentWaveClass);
                    }
                }
            }
        }
        
        private void SetRandomPositions()
        {
            foreach (KeyValuePair<GameObject, WaveClass> dictionary in lastSpawnedWaveDictionary)
            {
                GameObject currentEnemy = dictionary.Key;
                WaveClass currentWaveClass = dictionary.Value;
                
                currentEnemy.transform.position = GetRandomPositionOutsideColliders(transform.position, currentWaveClass.spawnRange);
            }
        }

        private Vector3 GetRandomPositionOutsideColliders(Vector3 basePosition, Vector3 spawnRange)
        {
            Vector3 randomPos = Vector3.zero;

            do
            {
                randomPos.x = Random.Range(basePosition.x - spawnRange.x, basePosition.x + spawnRange.x);
                randomPos.y = Random.Range(basePosition.y, basePosition.y + spawnRange.y);
                randomPos.z = Random.Range(basePosition.z - spawnRange.z, basePosition.z + spawnRange.z);
                    
                // If the object is spawned inside a collider, get a random position again.
                // The Vector3.up multiplier mustn't be greater than the radius. It can cause the editor to freeze.
            } while(Physics.CheckSphere(randomPos + Vector3.up * 3, 2f, collidableLayers));

            return randomPos;
        }

        public void StopSpawningWaves()
        {
            CancelInvoke();
        }
    }
}