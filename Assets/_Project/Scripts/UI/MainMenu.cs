using UnityEngine;

namespace _Project.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void QuitGame()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}