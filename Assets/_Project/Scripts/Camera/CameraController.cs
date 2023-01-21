using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float sensitivity;
        
        [Tooltip("This is the transform that camera will rotate around the y-axis.")]
        [SerializeField] private Transform orientation;


        float xRotation,
            yRotation;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        // This method is used by the player movement script.
        public Transform GetOrientationObject()
        {
            return orientation;
        }
    }
}