using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Fruits_8_CardsGame : MonoBehaviour
{
    public List<GameObject> cards;
    public List<Sprite> cardImages;
    public Sprite defaultCardIcon;

    public List<GameObject> matchPanels;
    private Dictionary<Sprite, GameObject> panelDictionary;
    private GameObject activePanel = null;

    private GameObject firstSelectedCard = null;
    private GameObject secondSelectedCard = null;
    private bool canFlip = true;
    private int matchesFound = 0;

    private int currentScore = 0;

    // UI references
    public GameObject scoreLabelGO;
    public GameObject coinImageGO;
    public Text pointsText;
    public GameObject timeLimitTextGO;

    // Sound effects
    public AudioClip matchSound;
    public AudioClip mismatchSound;
    public AudioClip gameWinSound;
    private AudioSource audioSource;

    private bool isEasyMode = false;
    private float timeLeft;
    private bool timerRunning = false;

    void Start()
    {
        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "NORMAL");
        isEasyMode = (difficulty == "EASY");

        if (isEasyMode)
        {
            if (scoreLabelGO != null) scoreLabelGO.SetActive(false);
            if (coinImageGO != null) coinImageGO.SetActive(false);
            if (pointsText != null) pointsText.gameObject.SetActive(false);
            if (timeLimitTextGO != null) timeLimitTextGO.SetActive(false);
        }
        else
        {
            if (scoreLabelGO != null) scoreLabelGO.SetActive(true);
            if (coinImageGO != null) coinImageGO.SetActive(true);
            if (pointsText != null) pointsText.gameObject.SetActive(true);
            if (timeLimitTextGO != null) timeLimitTextGO.SetActive(true);
        }

        // Set timer based on difficulty
        if (difficulty == "HARD")
        {
            timeLeft = 60f;
        }
        else if (difficulty == "NORMAL")
        {
            timeLeft = 120f;
        }
        else
        {
            timeLeft = 0f;
        }

        UpdateTimerText();

        if (!isEasyMode && timeLeft > 0)
        {
            timerRunning = true;
        }

        // Initialize panels
        foreach (var panel in matchPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        AssignImagesToCards();
        InitializePanels();
        LoadHighScoresToPanel();
        UpdatePointsText();
    }

    void Update()
    {
        if (timerRunning)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                UpdateTimerText();
                timerRunning = false;
                OnTimeUp();
            }
            else
            {
                UpdateTimerText();
            }
        }
    }

    void UpdateTimerText()
    {
        if (timeLimitTextGO != null)
        {
            Text timerText = timeLimitTextGO.GetComponent<Text>();
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeLeft / 60);
                int seconds = Mathf.FloorToInt(timeLeft % 60);
                int centiseconds = Mathf.FloorToInt((timeLeft * 100) % 100);
                timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, centiseconds);
            }
        }
    }

    void OnTimeUp()
    {
        DisableAllCards();
        EndGame();
    }

    void DisableAllCards()
    {
        foreach (var card in cards)
        {
            if (card != null)
            {
                Button btn = card.GetComponent<Button>();
                if (btn != null)
                    btn.interactable = false;
            }
        }
    }

    void AssignImagesToCards()
    {
        List<Sprite> shuffledImages = new List<Sprite>(cardImages);
        shuffledImages.AddRange(cardImages);
        shuffledImages = ShuffleList(shuffledImages);

        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = cards[i];
            GameObject icon = card.transform.GetChild(0).gameObject;
            Image iconImage = icon.GetComponent<Image>();
            Image cardImage = card.GetComponent<Image>();

            iconImage.sprite = defaultCardIcon;
            icon.SetActive(true);
            cardImage.sprite = shuffledImages[i];
            cardImage.enabled = false;

            Button cardButton = card.GetComponent<Button>();
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => ClickCard(card));
            cardButton.interactable = true;
        }
    }

    void InitializePanels()
    {
        panelDictionary = new Dictionary<Sprite, GameObject>();
        for (int i = 0; i < cardImages.Count; i++)
        {
            if (i < matchPanels.Count)
            {
                panelDictionary[cardImages[i]] = matchPanels[i];
                matchPanels[i].SetActive(false);
            }
        }
    }

    public void ClickCard(GameObject card)
    {
        if (card == firstSelectedCard || card == secondSelectedCard || !canFlip)
            return;

        if (!isEasyMode && !timerRunning && timeLeft <= 0)
            return;

        Animator animator = card.GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Highlight");

        FlipCard(card);

        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;
        }
        else
        {
            secondSelectedCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    void FlipCard(GameObject card)
    {
        GameObject icon = card.transform.GetChild(0).gameObject;
        Image cardImage = card.GetComponent<Image>();

        icon.SetActive(false);
        cardImage.enabled = true;
    }

    IEnumerator CheckMatch()
    {
        canFlip = false;
        yield return new WaitForSeconds(0.7f);

        Image firstImage = firstSelectedCard.GetComponent<Image>();
        Image secondImage = secondSelectedCard.GetComponent<Image>();

        if (firstImage.sprite == secondImage.sprite)
        {
            // Play match sound
            if (matchSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(matchSound);
            }

            firstSelectedCard.GetComponent<Button>().interactable = false;
            secondSelectedCard.GetComponent<Button>().interactable = false;
            matchesFound++;

            if (!isEasyMode)
            {
                currentScore += 20;
                UpdatePointsText();
            }

            ShowPanelForMatchedPair(firstImage.sprite);
            CheckForGameOver();
        }
        else
        {
            // Play mismatch sound
            if (mismatchSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(mismatchSound);
            }

            firstSelectedCard.transform.GetChild(0).gameObject.SetActive(true);
            secondSelectedCard.transform.GetChild(0).gameObject.SetActive(true);
            firstImage.enabled = false;
            secondImage.enabled = false;

            if (secondSelectedCard != null)
            {
                Animator anim = secondSelectedCard.GetComponent<Animator>();
                if (anim != null) anim.SetTrigger("Highlight");
            }
        }

        firstSelectedCard = null;
        secondSelectedCard = null;
        canFlip = true;
    }

    void ShowPanelForMatchedPair(Sprite matchedSprite)
    {
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }

        if (panelDictionary.ContainsKey(matchedSprite))
        {
            activePanel = panelDictionary[matchedSprite];
            activePanel.SetActive(true);
        }
    }

    private void CheckForGameOver()
    {
        if (matchesFound == cards.Count / 2)
        {
            timerRunning = false;

            // Play game win sound
            if (gameWinSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(gameWinSound);
            }

            if (!isEasyMode)
            {
                string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "NORMAL");

                if (difficulty == "HARD")
                {
                    currentScore += Mathf.RoundToInt(timeLeft * 400);
                }
                else
                {
                    currentScore += Mathf.RoundToInt(timeLeft * 20);
                }

                UpdatePointsText();
                SaveScore(currentScore);
                LoadHighScoresToPanel();
            }

            DisableAllCards();
            GameObject.Find("ContinueManager").GetComponent<ContinueManager>().ShowContinueButton();
        }
    }

    private void EndGame()
    {
        if (!isEasyMode)
        {
            SaveScore(currentScore);
            LoadHighScoresToPanel();
        }
        DisableAllCards();
        GameObject.Find("ContinueManager").GetComponent<ContinueManager>().ShowContinueButton();
    }

    private void SaveScore(int score)
    {
        List<int> scores = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            scores.Add(PlayerPrefs.GetInt("Fruits_8_Score_" + i, 0));
        }

        scores.Add(score);
        scores = scores.OrderByDescending(s => s).Take(10).ToList();

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt("Fruits_8_Score_" + i, scores[i]);
        }

        PlayerPrefs.Save();
    }

    private void LoadHighScoresToPanel()
    {
        // Implement high score display if needed
    }

    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = currentScore.ToString();
        }
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}