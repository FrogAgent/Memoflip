using UnityEngine;
using System.Collections;

public class PanelEvents : MonoBehaviour
{
    public float delayBeforeDisable = 2f; // seconds to wait before disabling panel

    public void DisablePanelWithDelay()
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDisable);
        gameObject.SetActive(false);
    }
}
