using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    // This script handles the resetting of the current scene.
    public class ScenesManager : MonoBehaviour
    {
        public int sceneIndex;

        public void OnResetClick()
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SceneChange()
        {
            // Load the next scene
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
