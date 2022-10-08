using UnityEngine;

namespace Camera
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlaceholderPosition;
    
        void Update()
        {
            transform.position = cameraPlaceholderPosition.position;
        }
    }
}
