using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Managers
{
	public class GameObjectPool : MonoBehaviour
	{
		[Header("Pool Properties")]
		[SerializeField] private GameObject poolObjectPrefab;
		[SerializeField] private int initialPoolSize;

		readonly Queue<GameObject> availablePoolObjects = new();

		[Header("Transform Properties")] 
		[SerializeField] private Transform parentTransform;
		
		void Awake()
		{
			GrowPoolBy(initialPoolSize);
		}

		private void GrowPoolBy(int objectCount)
		{
			for (int i = 0; i < objectCount; i++)
			{
				GameObject newPoolObject = Instantiate(poolObjectPrefab, parentTransform);

				AssignNewPoolObjectProperties(newPoolObject);

				AddToQueue(newPoolObject);
			}
		}

		private void AssignNewPoolObjectProperties(GameObject newPoolObject)
		{
			// Assign this script for the pool element to return to the pool.
			newPoolObject.GetComponentInChildren<GameObjectPoolElement>().ownerPoolScript = this;
		}

		public void AddToQueue(GameObject poolObject)
		{
			DisableObject(poolObject);
			
			availablePoolObjects.Enqueue(poolObject);
		}

		private void DisableObject(GameObject poolObject)
		{
			poolObject.SetActive(false);
		}

		public List<GameObject> GetFromQueue(int count)
		{
			if (count <= 0)
				throw new Exception("Cannot get 0 or negative number objects from the pool");

			// Create new objects if there arent enough in the pool.
			if (availablePoolObjects.Count < count)
			{
				GrowPoolBy(count - availablePoolObjects.Count);
			}
			

			List<GameObject> chosenPoolObjectsList = new();
			
			for (int i = 0; i < count; i++)
			{
				GameObject chosenPoolObject = availablePoolObjects.Dequeue();

				EnableObject(chosenPoolObject);

				chosenPoolObjectsList.Add(chosenPoolObject);
			}

			return chosenPoolObjectsList;
		}

		private void EnableObject(GameObject poolObject)
		{
			poolObject.SetActive(true);
		}
	}
}
