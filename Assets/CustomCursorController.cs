using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomCursorController : MonoBehaviour
{
    [Tooltip("Assign the RectTransform of your custom cursor UI Image here")]
    public RectTransform cursorRectTransform;

    [Tooltip("Speed multiplier for cursor movement")]
    public float cursorSpeed = 1f;

    // Current virtual cursor position in screen coordinates
    private Vector2 virtualCursorPos;

    // For tracking click events
    private bool isPointerDown = false;

    void Start()
    {
        if (cursorRectTransform == null)
        {
            Debug.LogError("Cursor RectTransform is not assigned!");
            enabled = false;
            return;
        }

        Cursor.visible = false;

        // Start cursor at center of screen
        virtualCursorPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        cursorRectTransform.position = virtualCursorPos;
    }

    void Update()
    {
        // Get raw mouse delta input
        float deltaX = Input.GetAxisRaw("Mouse X");
        float deltaY = Input.GetAxisRaw("Mouse Y");

        // Move virtual cursor by delta times speed, scale for frame time for smoothness
        virtualCursorPos += new Vector2(deltaX, deltaY) * cursorSpeed * 10f;

        // Clamp inside screen bounds
        virtualCursorPos.x = Mathf.Clamp(virtualCursorPos.x, 0, Screen.width);
        virtualCursorPos.y = Mathf.Clamp(virtualCursorPos.y, 0, Screen.height);

        // Update cursor position
        cursorRectTransform.position = virtualCursorPos;

        // Handle pointer events
        HandlePointerEvents();
    }

    private void HandlePointerEvents()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = virtualCursorPos;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // Pointer Down
        if (Input.GetMouseButtonDown(0) && !isPointerDown)
        {
            isPointerDown = true;
            foreach (var result in raycastResults)
            {
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                break; // only first UI element
            }
        }

        // Pointer Up
        if (Input.GetMouseButtonUp(0) && isPointerDown)
        {
            isPointerDown = false;
            foreach (var result in raycastResults)
            {
                ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
                break;
            }
        }
    }
}
