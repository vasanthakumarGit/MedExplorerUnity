using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        // Get the currently active scene's build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the scene by referencing its build index
        SceneManager.LoadScene(currentSceneIndex);
    }
}
