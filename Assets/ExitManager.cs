using UnityEngine;

public class ExitManager : MonoBehaviour
{
    // This method will be called when the exit button is clicked
    public void OnExitButtonClick()
    {
        Debug.Log("Exit Button Clicked!");

        // Close the application
        Application.Quit();

        // If running in the editor, stop the play mode (Unity Editor only)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
