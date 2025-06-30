using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("Assign 9 custom cursors (excluding default system cursor)")]
    public Texture2D[] customCursors = new Texture2D[9]; // Index 1–9
    public Vector2 hotspot = Vector2.zero;

    private const string CursorPrefKey = "SelectedCursorIndex";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load and apply cursor at game start
            int savedIndex = PlayerPrefs.GetInt(CursorPrefKey, 0);
            ApplySavedCursor(savedIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplySavedCursor(int index)
    {
        if (index == 0)
        {
            // Default system cursor
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else if (index >= 1 && index <= customCursors.Length)
        {
            Texture2D selectedCursor = customCursors[index - 1];
            if (selectedCursor != null)
            {
                Cursor.SetCursor(selectedCursor, hotspot, CursorMode.Auto);
            }
            else
            {
                Debug.LogWarning($"Cursor texture at index {index - 1} not assigned.");
            }
        }
    }
}
