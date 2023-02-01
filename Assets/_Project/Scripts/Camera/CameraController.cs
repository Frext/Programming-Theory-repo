using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Properties")]
        [SerializeField] private float mouseSensitivity;

        [Serializable]
        private class CameraClass
        {
            public GameObject cameraObject;
            
            [Space]
            public bool rotateX;
        }
        
        [SerializeField] private List<CameraClass> cameraClassList;
        
        int _currentCameraTransformIndex;
        int CurrentCameraTransformIndex
        {
            get => _currentCameraTransformIndex;
            set
            {
                // Rewind the index value to 0 if the set value equals to the count of the camera transforms.
                if (value == cameraClassList.Count)
                {
                    _currentCameraTransformIndex = 0;
                }
                else
                {
                    _currentCameraTransformIndex = Mathf.Clamp(value, 0, cameraClassList.Count - 1);
                }

                UpdateCameraActiveStates();
            }
        }

        private void UpdateCameraActiveStates()
        {
            for (int index = 0; index < cameraClassList.Count; index++)
            {
                // If the current index is found, turn the camera object on.
                cameraClassList[index].cameraObject.SetActive(index == CurrentCameraTransformIndex);
            }
        }


        [Header("Player Properties")]
        
        [Tooltip("This is the transform that camera will rotate around the y-axis.")]
        [SerializeField] private Transform orientation;


        float xRotation,
            yRotation;

        
        void Start()
        {
            CurrentCameraTransformIndex = 0;
            
            LockCursor();
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            HandleCameraSwitch();
            
            GetMouseInput();
            
            SetRotation();
        }

        private void HandleCameraSwitch()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                CurrentCameraTransformIndex ++;
            }
        }
        
        private void GetMouseInput()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            ApplyCurrentCameraRotationPreferences();
        }

        private void ApplyCurrentCameraRotationPreferences()
        {
            // If the camera doesn't rotate in the x-axis, set the x rotation to 0.
            if (!cameraClassList[CurrentCameraTransformIndex].rotateX)
                xRotation = 0;
        }
        
        private void SetRotation()
        {
            cameraClassList[CurrentCameraTransformIndex].cameraObject.transform.rotation = 
                Quaternion.Euler(xRotation, yRotation, 0);
           
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        #region Methods Used By Other Scripts
        
        // This method is used when the game over UI is showed to activate the cursor.
        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // This method is used by the player movement script.
        public Transform GetOrientationObject()
        {
            return orientation;
        }

        #endregion
    }
}