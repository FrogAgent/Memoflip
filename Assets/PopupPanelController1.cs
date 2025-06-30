using UnityEngine;

public class PopupPanelController1 : MonoBehaviour
{
    public GameObject confirmPanel; // Assign ConfirmPanel here

    void Start()
    {
        confirmPanel.SetActive(false); // Hide at start
    }

    public void OpenPanel()
    {
        confirmPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        confirmPanel.SetActive(false);
    }
}