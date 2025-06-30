using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TextGameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject[] infoPanels;
    public GameObject[] questionPanels;

    [Header("Animation Control")]
    public Animator panelAnimator;
    [SerializeField] private string entranceTrigger = "PanelEnter";
    [SerializeField] private string exitTrigger = "PanelExit";
    [SerializeField] private float animationExitDelay = 2f;
    [SerializeField] private float questionPanelShowDelay = 2f;
    [SerializeField] private float correctAnswerDelay = 2f;

    [Header("Sound Effects")]
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioClip questionAppearSound;
	public AudioClip exitAnimationSound;
    private AudioSource audioSource;

    private GameObject activeInfoPanel;
    private GameObject activeQuestionPanel;
    private Text[] questionTextObjects;
    private int selectedQuestionIndex;
    private InputField answerInput;
    private Text feedbackText;
    private List<string> currentAnswers;
    private int currentInfoPanelIndex = -1;
    private int wrongAttemptCount = 0;
    private const int maxWrongAttempts = 10;
    private bool isTransitioning = false;
    private bool inputDisabled = false;

    void Start()
    {
        InitializeAudio();
        HideAllPanels();
        ShowRandomInfoPanel();
    }

    void InitializeAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource component added to " + gameObject.name);
        }
    }

    void Update()
    {
        if (inputDisabled) return;

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && 
            answerInput != null && answerInput.isFocused)
        {
            OnSubmitAnswer();
        }
    }

    void HideAllPanels()
    {
        foreach (var panel in infoPanels) panel.SetActive(false);
        foreach (var panel in questionPanels) panel.SetActive(false);
    }

    void ShowRandomInfoPanel()
    {
        currentInfoPanelIndex = Random.Range(0, infoPanels.Length);
        activeInfoPanel = infoPanels[currentInfoPanelIndex];
        activeInfoPanel.SetActive(true);

        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(entranceTrigger);
        }
    }

    public void OnContinueToQuestionPanel()
    {
        if (isTransitioning || inputDisabled) return;
        
        PanelEvents panelEvents = activeInfoPanel.GetComponent<PanelEvents>();
        if (panelEvents != null)
        {
            panelEvents.DisablePanelWithDelay();
        }
        else
        {
            activeInfoPanel.SetActive(false);
        }

        StartCoroutine(ShowQuestionPanelWithDelay(currentInfoPanelIndex));
    }

    private IEnumerator ShowQuestionPanelWithDelay(int panelIndex)
    {
        isTransitioning = true;
        yield return new WaitForSeconds(questionPanelShowDelay);
        ShowQuestionPanel(panelIndex);
        isTransitioning = false;
    }

    void ShowQuestionPanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= questionPanels.Length)
        {
            Debug.LogError("Invalid panel index: " + panelIndex);
            return;
        }

        activeQuestionPanel = questionPanels[panelIndex];
        activeQuestionPanel.SetActive(true);

        PlaySound(questionAppearSound);

        questionTextObjects = activeQuestionPanel.GetComponentsInChildren<Text>(true);
        answerInput = activeQuestionPanel.GetComponentInChildren<InputField>(true);
        feedbackText = null;

        List<Text> questionTexts = new List<Text>();
        foreach (Text txt in questionTextObjects)
        {
            if (txt.name.ToLower().Contains("questiontext"))
                questionTexts.Add(txt);
        }

        if (questionTexts.Count == 0)
        {
            Debug.LogError("No question texts found in " + activeQuestionPanel.name);
            return;
        }

        foreach (Text q in questionTexts) q.gameObject.SetActive(false);
        selectedQuestionIndex = Random.Range(0, questionTexts.Count);
        questionTexts[selectedQuestionIndex].gameObject.SetActive(true);

        foreach (Text t in activeQuestionPanel.GetComponentsInChildren<Text>(true))
        {
            if (t.name.ToLower().Contains("feedback"))
            {
                feedbackText = t;
                break;
            }
        }

        if (feedbackText != null) feedbackText.text = "";
        currentAnswers = GetAnswersForPanelAndQuestion(activeQuestionPanel.name, selectedQuestionIndex);
    }

    public void OnSubmitAnswer()
    {
        if (isTransitioning || inputDisabled) return;
        
        string userAnswer = answerInput.text.Trim().ToLower();

        if (IsAnswerCorrect(userAnswer))
        {
            HandleCorrectAnswer();
        }
        else
        {
            HandleWrongAnswer();
        }
    }

    private bool IsAnswerCorrect(string userAnswer)
    {
        foreach (string correctAnswer in currentAnswers)
        {
            if (userAnswer == correctAnswer.ToLower()) return true;
        }
        return false;
    }

    private void HandleCorrectAnswer()
    {
        PlaySound(correctAnswerSound);
        feedbackText.text = "Correct ☻";
        isTransitioning = true;
        inputDisabled = true;

        if (answerInput != null)
        {
            answerInput.interactable = false;
        }
        
        if (activeQuestionPanel != null)
        {
            Button[] buttons = activeQuestionPanel.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }

        StartCoroutine(TriggerExitSequence());
    }

    private IEnumerator TriggerExitSequence()
    {
        yield return new WaitForSeconds(correctAnswerDelay);
        
		// Play exit animation sound
        PlaySound(exitAnimationSound);
		
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger(exitTrigger);
        }
        
        yield return new WaitForSeconds(animationExitDelay);
        LoadGameOver1Scene();
    }

    private void HandleWrongAnswer()
    {
        PlaySound(wrongAnswerSound);
        wrongAttemptCount++;
        feedbackText.text = $"Wrong Attempts left: {maxWrongAttempts - wrongAttemptCount}";

        if (wrongAttemptCount >= maxWrongAttempts)
        {
            inputDisabled = true;
            isTransitioning = true;
            
            if (answerInput != null)
            {
                answerInput.interactable = false;
            }
            
            if (activeQuestionPanel != null)
            {
                Button[] buttons = activeQuestionPanel.GetComponentsInChildren<Button>(true);
                foreach (Button button in buttons)
                {
                    button.interactable = false;
                }
            }
            
            Invoke("LoadGameOver2Scene", 2f);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Missing audio reference: " + 
                           (audioSource == null ? "AudioSource" : "AudioClip"));
        }
    }

    void LoadGameOver1Scene()
    {
        SceneManager.LoadScene("GameOver1Scene");
    }

    void LoadGameOver2Scene()
    {
        SceneManager.LoadScene("GameOver2Scene");
    }

    List<string> GetAnswersForPanelAndQuestion(string panelName, int questionIndex)
    {
        if (panelName.Contains("QuestionPanel1"))
        {
            switch (questionIndex)
            {
                case 0: return new List<string> { "1969" };
                case 1: return new List<string> { "michael collins" };
                case 2: return new List<string> { "that's one small step for man, one giant leap for mankind.", "that's one small step for man, one giant leap for mankind", "that's one small step for man,one giant leap for mankind", "that's one small step for man,one giant leap for mankind." };
                case 3: return new List<string> { "21.5 kg", "47.5 pounds", "21.5kg", "47.5pounds", "21,5 kg", "47,5 pounds", "21,5kg", "47,5pounds" };
                case 4: return new List<string> { "usa,soviet union", "usa , soviet union", "usa and soviet union", "usa,USSR", "usa , USSR", "united states , USSR", "united states and soviet union", "usa and USSR", "united states and USSR" };
            }
        }
        else if (panelName.Contains("QuestionPanel2"))
        {
            switch (questionIndex)
            {
                case 0: return new List<string> { "1912" };
                case 1: return new List<string> { "an iceberg", "iceberg" };
                case 2: return new List<string> { "over 1500", "more than 1500", "1500", "1.500", "1,500", "over 1.500", "more than 1.500", "over 1,500", "more than 1,500" };
                case 3: return new List<string> { "robert ballard" };
                case 4: return new List<string> { "new york city", "new york" };
            }
        }
        else if (panelName.Contains("QuestionPanel3"))
        {
            switch (questionIndex)
            {
                case 0: return new List<string> { "2560", "2560 bce", "2560bce", "2560bc" };
                case 1: return new List<string> { "pharaoh khufu", "khufu" };
                case 2: return new List<string> { "2.3 million", "2.3million", "2,3 million", "2,3million", "2.3" };
                case 3: return new List<string> { "3,800 years", "3,800", "3.800", "3800", "3.800 years", "3.800years", "3,800years", "3800 years", "3800years" };
                case 4: return new List<string> { "exact construction method", "the exact construction method" };
            }
        }
        else if (panelName.Contains("QuestionPanel4"))
        {
            switch (questionIndex)
            {
                case 0: return new List<string> { "thomas edison", "edison" };
                case 1: return new List<string> { "1879" };
                case 2: return new List<string> { "carbon filament", "carbon" };
                case 3: return new List<string> { "use of electricity", "electricity" };
                case 4: return new List<string> { "1882" };
            }
        }
        else if (panelName.Contains("QuestionPanel5"))
        {
            switch (questionIndex)
            {
                case 0: return new List<string> { "nepal", "china" };
                case 1: return new List<string> { "8848", "8848 meters", "8848meters", "8848m", "8848 m", "8.848", "8.848m", "8.848 m", "8.848 meters", "8.848meters" };
                case 2: return new List<string> { "1953" };
                case 3: return new List<string> { "sherpas" };
                case 4: return new List<string> { "edmund hillary,tenzing norgay", "edmund hillary , tenzing norgay", "edmund hillary, tenzing norgay", "edmund hillary and tenzing norgay" };
            }
        }

        return new List<string> { "unknown" };
    }
}