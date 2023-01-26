using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Managers
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
            [Space]
            public Vector3 spawnRange;
        }
        
        [SerializeField] List<WaveClass> waveClasses;

        [Space]
        [SerializeField] private IntObject WaveCountSO;
        [Range(1, 10)] [SerializeField] private float waveInterval = 2f;
        
        
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
            foreach (GameObject currentEnemy in lastSpawnedWaveDictionary.Keys)
            {
                if (currentEnemy.activeInHierarchy)
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
                
                Vector3 currentPos = currentEnemy.transform.position;
                Vector3 waveManagerPos = transform.position;

                Vector3 spawnRange = dictionary.Value.spawnRange;

                currentPos.x = Random.Range(waveManagerPos.x - spawnRange.x, waveManagerPos.x + spawnRange.x);
                currentPos.z = Random.Range(waveManagerPos.z - spawnRange.z, waveManagerPos.z + spawnRange.z);

                currentEnemy.transform.position = currentPos;
            }
        }

        public void StopSpawningWaves()
        {
            CancelInvoke();
        }
    }
}