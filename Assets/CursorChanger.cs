using UnityEngine;
using UnityEngine.UI;

public class CursorChanger : MonoBehaviour
{
    [Header("Assign 10 cursor textures below (excluding default system cursor)")]
    public Texture2D[] customCursors = new Texture2D[9]; // Index 1-9 for custom cursors

    public Text cursorLabel; // Assign UI Text for cursor name display
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private int currentIndex;
    private string[] cursorNames = {
        "Cursor 1", "Cursor 2", "Cursor 3", "Cursor 4", "Cursor 5",
        "Cursor 6", "Cursor 7", "Cursor 8", "Cursor 9", "Cursor 10"
    };

    private const string CursorPrefKey = "SelectedCursorIndex";

    void Start()
    {
        currentIndex = PlayerPrefs.GetInt(CursorPrefKey, 0); // Load saved index or default to 0
        ApplyCursor(currentIndex);
    }

    public void NextCursor()
    {
        currentIndex = (currentIndex + 1) % 10;
        ApplyCursor(currentIndex);
        SaveCursorIndex();
    }

    public void PreviousCursor()
    {
        currentIndex = (currentIndex - 1 + 10) % 10;
        ApplyCursor(currentIndex);
        SaveCursorIndex();
    }

    public void ApplyCursor(int index)
    {
        if (index == 0)
        {
            // Use system default cursor
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
        else if (index >= 1 && index <= customCursors.Length)
        {
            Texture2D selectedCursor = customCursors[index - 1];
            if (selectedCursor != null)
                Cursor.SetCursor(selectedCursor, hotspot, cursorMode);
            else
                Debug.LogWarning("Cursor " + index + " is not assigned.");
        }

        if (cursorLabel != null && index >= 0 && index < cursorNames.Length)
        {
            cursorLabel.text = cursorNames[index];
        }
    }

    private void SaveCursorIndex()
    {
        PlayerPrefs.SetInt(CursorPrefKey, currentIndex);
        PlayerPrefs.Save();
    }
}
