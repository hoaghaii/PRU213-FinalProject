using UnityEngine;
using UnityEngine.SceneManagement;

namespace Animation2D.Core
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadNextScene()
        {
            int next = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(next);
        }

        public void LoadSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

