using UnityEngine;
using UnityEngine.SceneManagement;

public class BackManager : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
} 