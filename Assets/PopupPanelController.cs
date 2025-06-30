using UnityEngine;

public class PopupPanelController : MonoBehaviour
{
    public GameObject overlayPanel; // Assign OverlayPanel here

    void Start()
    {
        overlayPanel.SetActive(false); // Hide at start
    }

    public void OpenPanel()
    {
        overlayPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        overlayPanel.SetActive(false);
    }
}
