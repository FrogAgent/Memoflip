using UnityEngine;

public class BackManager1 : MonoBehaviour
{
    [SerializeField] private GameObject panelToShow;     // The panel you want to show
    [SerializeField] private GameObject panelToHide;     // (Optional) The panel you want to hide

    public void ShowPanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }

        if (panelToHide != null)
        {
            panelToHide.SetActive(false);
        }
    }
}
