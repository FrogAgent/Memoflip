using UnityEngine;

public class SceneNavigationTracker : MonoBehaviour
{
    // Static variables to store scene navigation information
    public static bool CameFromPauseMenu = false; // Tracks if the user came from the pause menu
    public static string LastGameSceneName = ""; // Stores the last game scene's name

    // This ensures the tracker exists in the scene across scene loads
    private void Awake()
    {
        // If no instance exists, make sure it's not destroyed on scene load
        if (FindObjectsOfType<SceneNavigationTracker>().Length > 1)
        {
            Destroy(gameObject); // If there is already an instance, destroy this duplicate
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Don't destroy on scene load
        }
    }
}
