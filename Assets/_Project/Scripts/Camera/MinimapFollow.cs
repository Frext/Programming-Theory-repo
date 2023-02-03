using UnityEngine;

namespace _Project.Scripts.Camera
{
	public class MinimapFollow : MonoBehaviour
	{
		[SerializeField] private Transform targetTransform;
		[SerializeField] private Vector3 targetOffset;
		
		
		void LateUpdate()
		{
			UpdatePosition();
			UpdateRotation();
		}
		
		private void UpdatePosition()
		{
			transform.position = targetTransform.position + targetOffset;
		}
		
		private void UpdateRotation()
		{
			Vector3 newEulerAngles = transform.eulerAngles;
			newEulerAngles.y = targetTransform.eulerAngles.y;
			
			transform.localRotation = Quaternion.Euler(newEulerAngles);
		}
	}
}
