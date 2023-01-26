using System;
using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Pickups
{
	[RequireComponent(typeof(Collider))]
	public class HealthPickup : MonoBehaviour
	{
		[Range(0,10)] [SerializeField] private int healAmount;
		
		[SerializeField] private LayerMask healLayers;
		[Space]
		[SerializeField] private UnityEvent onPickup;

		private void OnTriggerStay(Collider other)
		{
			if (IsInLayer(healLayers, other.gameObject.layer))
			{
				HealthManager healthManager = other.gameObject.GetComponent<HealthManager>();

				bool didHeal = true;
				
				if (healthManager != null)
				{
					didHeal = healthManager.AddHealth(healAmount);
				}
				
				// If the player have full health, don't waste the health pickup. 
				if (didHeal)
				{
					onPickup.Invoke();
				}
			}
		}
		
		private bool IsInLayer(LayerMask layerMask,int layer)
		{
			// Returns true if the layer that is converted into a layer mask and the attack layer mask have a common bit which is 1.
			return (layerMask & (1 << layer)) != 0;
		}
	}
}
