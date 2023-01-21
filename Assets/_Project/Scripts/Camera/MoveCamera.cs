using UnityEngine;

namespace Camera
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlaceholder;
    
        void Update()
        {
            transform.position = cameraPlaceholder.position;
        }
    }
}
