using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraPlaceholder;
    
        void LateUpdate()
        {
            transform.position = cameraPlaceholder.position;
        }
    }
}
