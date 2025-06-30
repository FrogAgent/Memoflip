using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public Image cardImage;  // Image component to display the card's current image
    public Sprite defaultCardIcon;  // Back side image (before clicking)
    private Sprite frontImage;  // The front image (after clicking)
    private bool isFlipped = false;  // Flag to track card state
    private bool isMatched = false;  // Flag to check if card is matched

    // Set the front image for this card
    public void SetCardImage(Sprite image)
    {
        frontImage = image;  // Store the front image
        cardImage.sprite = defaultCardIcon;  // Start with the card back
    }

    // Flip the card when clicked
    public void FlipCard()
    {
        if (isMatched) return;  // Do nothing if already matched

        isFlipped = !isFlipped;  // Toggle card state
        cardImage.sprite = isFlipped ? frontImage : defaultCardIcon;  // Change sprite
    }

    // Get the front image of the card
    public Sprite GetCardImage()
    {
        return frontImage;
    }

    // Set the card as matched (so it stays flipped)
    public void SetMatched(bool matched)
    {
        isMatched = matched;
    }

    // Check if the card is matched
    public bool IsMatched()
    {
        return isMatched;
    }
}
