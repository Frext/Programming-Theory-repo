using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Managers
{
    [Serializable]
    public class ObjectPoolClass
    {
        public GameObjectPool gameObjectPool;
        [Range(0, 10)] public int initialWaveSize;

        [Space] 
        [Range(0, 10)] public int waveSizeIncreaseCount;
        [Range(0, 10)] public int increaseWaveSizeAfterWaves;
    }

    public class EnemyWaveManager : MonoBehaviour
    {
        [Header("Wave Properties")]
        [Space]
        [SerializeField]
        List<ObjectPoolClass> objectPoolClasses;

        [SerializeField] private Vector3 spawnRange;

        [Space] [SerializeField] private IntObject WaveCount;

        List<GameObject> lastSpawnedWaveList = new();

        void Awake()
        {
            if (objectPoolClasses.Count == 0)
                enabled = false;
        }

        void Start()
        {
            WaveCount.value = 0;
            
            InvokeRepeating(nameof(GenerateWaveWhenDead), 1f, 1f);
        }

        private void GenerateWaveWhenDead()
        {
            if (IsCurrentWaveDead())
            {
                GenerateNewWave();
                SetRandomPositions();
            }
        }

        private void GenerateNewWave()
        {
            WaveCount.value++;
            
            lastSpawnedWaveList.Clear();

            foreach (ObjectPoolClass currentPoolClass in objectPoolClasses)
            {
                // We subtract 1 from the WaveCount to equalize the right side of the sum to 0
                // for the initial wave size at the 1st wave.
                int waveSize = currentPoolClass.initialWaveSize + currentPoolClass.waveSizeIncreaseCount *
                    ((WaveCount.value - 1) / currentPoolClass.increaseWaveSizeAfterWaves);
                
                // We do this check not to get an exception from the game object pool.
                if (waveSize > 0)
                {
                    lastSpawnedWaveList.AddRange(currentPoolClass.gameObjectPool.GetFromQueue(waveSize));
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
    }
}