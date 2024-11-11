using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;  // Import UI namespace

public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // Check if the "ResetObject" button was pressed
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            ReloadScene();
        }
    }

    // Method to reload the active scene
    private void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
