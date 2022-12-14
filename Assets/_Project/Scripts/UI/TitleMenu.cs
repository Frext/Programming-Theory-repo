using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class TitleMenu : MonoBehaviour
    {
        // This is used by the play button.
        public void GoToNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
