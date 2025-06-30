using UnityEngine;
using System.Collections; // Add this for IEnumerator

public class CardAnim : MonoBehaviour
{
    public Sprite frontImage;  // Front image (before clicked)
    public Sprite backImage;   // Back image (after clicked)
    private SpriteRenderer spriteRenderer;
    private bool isFlipped = false;
    private bool isInteractable = true;

    private Quaternion frontRotation = Quaternion.Euler(0, 0, 0);
    private Quaternion backRotation = Quaternion.Euler(0, 180f, 0);

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing on " + gameObject.name);
        }
    }

    void Start()
    {
        FlipToFront();  // Initialize the card facing front
    }

    public void Flip()
    {
        if (!isInteractable) return;

        // Flip the card (change the sprite)
        isFlipped = !isFlipped;
        spriteRenderer.sprite = isFlipped ? backImage : frontImage;

        // Rotate the card manually without DOTween
        Quaternion targetRotation = isFlipped ? backRotation : frontRotation;
        StartCoroutine(RotateCard(targetRotation));
    }

    public void FlipToFront()
    {
        // Ensure the card is facing front initially
        if (spriteRenderer != null)
        {
            isFlipped = false;
            spriteRenderer.sprite = frontImage;
            transform.rotation = frontRotation; // Set rotation to front
        }
    }

    public void EnableCardInteraction()
    {
        isInteractable = true;
    }

    public void DisableCardInteraction()
    {
        isInteractable = false;
    }

    private IEnumerator RotateCard(Quaternion targetRotation)
    {
        // Smooth rotation towards the target
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;
        float duration = 0.25f; // Rotation duration

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure the final rotation is set
    }

    private void OnMouseDown()
    {
        if (isInteractable && SequenceGameManager.Instance != null)
        {
            SequenceGameManager.Instance.OnCardClick(this);
        }
    }
}
