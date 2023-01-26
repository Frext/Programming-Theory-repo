using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Managers
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [Serializable]
        private class EnemyWaveClass
        {
            public GameObjectPool gameObjectPool;
            
            [Header("Wave Properties")]
            [Range(0, 10)] public int initialWaveSize;
            [Space]
            [Range(0, 10)] public int waveSizeIncreaseCount;
            [Range(0, 10)] public int increaseWaveSizeAfterWaves;
        }
        
        [Header("Wave Properties")]
        [SerializeField] List<EnemyWaveClass> enemyWaveClasses;
        [SerializeField] private Vector3 spawnRange;
        
        [Space]
        [SerializeField] private IntObject WaveCountSO;
        [Range(1, 10)] [SerializeField] private float waveInterval = 2f;

        
        List<GameObject> lastSpawnedWaveList = new();

        void Awake()
        {
            if (enemyWaveClasses.Count == 0)
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
            foreach (GameObject currentEnemy in lastSpawnedWaveList)
            {
                if (currentEnemy.activeInHierarchy)
                    return false;
            }

            return true;
        }

        private void GenerateNewWave()
        {
            WaveCountSO.value++;
            
            lastSpawnedWaveList.Clear();

            foreach (EnemyWaveClass currentPoolClass in enemyWaveClasses)
            {
                // We subtract 1 from the WaveCount to equalize the right side of the sum to 0
                // for the initial wave size at the 1st wave.
                int waveSize = currentPoolClass.initialWaveSize + currentPoolClass.waveSizeIncreaseCount *
                    ((WaveCountSO.value - 1) / currentPoolClass.increaseWaveSizeAfterWaves);
                
                // We do this check not to get an exception from the game object pool.
                if (waveSize > 0)
                {
                    lastSpawnedWaveList.AddRange(currentPoolClass.gameObjectPool.GetFromQueue(waveSize));
                }
            }
            
            
        }
        
        private void SetRandomPositions()
        {
            foreach (GameObject currentEnemy in lastSpawnedWaveList)
            {
                Vector3 currentPos = currentEnemy.transform.position;
                Vector3 waveManagerPos = transform.position;

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