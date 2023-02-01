using _Project.Scripts.Gameplay.Characters.Player;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Pickups
{
	[RequireComponent(typeof(Collider))]
	public class HealthPickup : MonoBehaviour
	{
		[Range(0,20)] [SerializeField] private int healAmount;
		[SerializeField] private LayerMask healableLayers;
		
		[Space]
		[SerializeField] private UnityEvent onPickup;

		
		void OnTriggerStay(Collider other)
		{
			if (!HelperMethodsUtil.IsLayerInLayerMask(other.gameObject.layer, healableLayers)) 
				return;
			
			
			PlayerHealth healthManager = other.gameObject.GetComponent<PlayerHealth>();

			bool didHeal = false;
				
			if (healthManager != null)
			{
				didHeal = healthManager.IncreaseHealth(healAmount);
			}
				
			// If the player have full health, don't waste the health pickup. 
			if (didHeal)
			{
				onPickup.Invoke();
			}
		}
	}
}