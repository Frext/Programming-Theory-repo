using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        public void GoToRelativeScene(int relativeSceneIndex)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + relativeSceneIndex);
        }
    }
}
