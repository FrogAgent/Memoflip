using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SequenceGameManager : MonoBehaviour
{
    public static SequenceGameManager Instance; // Singleton for easy access

    public List<CardAnim> cards;  // List of all cards in the layout
    private List<CardAnim> correctSequence = new List<CardAnim>();  // Sequence of cards shown to the player
    private List<CardAnim> playerSequence = new List<CardAnim>();  // Sequence clicked by the player
    private bool gameActive = false;  // Is the game active (i.e., can the player click)
    private int currentSequenceLength = 2;  // Start with a sequence length of 2 cards

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cards = new List<CardAnim>(FindObjectsOfType<CardAnim>());

        // Ensure all cards start with their front image
        foreach (var card in cards)
        {
            card.FlipToFront();
        }

        StartCoroutine(StartSequenceGame());
    }

    IEnumerator StartSequenceGame()
    {
        playerSequence.Clear();
        gameActive = false;  // Disable player interaction

        // Ensure all cards start face-down before showing sequence
        foreach (var card in cards)
        {
            card.FlipToFront();
            card.DisableCardInteraction(); // Prevent clicks during sequence
        }

        yield return new WaitForSeconds(1f);

        GenerateSequence();
        yield return StartCoroutine(ShowSequence());

        // After sequence is shown, enable interaction
        foreach (var card in cards)
        {
            card.EnableCardInteraction();
        }

        gameActive = true;
    }

    void GenerateSequence()
    {
        correctSequence.Clear();
        for (int i = 0; i < currentSequenceLength; i++)
        {
            correctSequence.Add(cards[Random.Range(0, cards.Count)]);
        }
    }

    IEnumerator ShowSequence()
    {
        yield return new WaitForSeconds(1f);

        foreach (CardAnim card in correctSequence)
        {
            card.Flip();  // Show the card
            yield return new WaitForSeconds(1f);
            card.Flip();  // Flip it back
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
    }

    public void OnCardClick(CardAnim clickedCard)
    {
        if (!gameActive || playerSequence.Contains(clickedCard)) return;

        playerSequence.Add(clickedCard);
        clickedCard.Flip();

        if (playerSequence.Count == correctSequence.Count)
        {
            CheckSequence();
        }
    }

    void CheckSequence()
    {
        bool correct = true;
        for (int i = 0; i < correctSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("Correct Sequence! Next Round...");
            playerSequence.Clear();
            currentSequenceLength = Mathf.Min(currentSequenceLength + 1, 14);
            StartCoroutine(StartSequenceGame());
        }
        else
        {
            Debug.Log("Wrong Sequence! Game Over!");
            SceneManager.LoadScene("GameOver1Scene");
        }
    }
}
