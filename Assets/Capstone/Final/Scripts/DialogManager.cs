using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager: MonoBehaviour
{
    public GameObject learnMenuPanel;
    public GameObject contentDisplayPanel;
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI dialogueTopicText;
    public Image dialogueTopicImage;
    public GameObject continueButton;
    public Button pauseResumeButton;
    public Button playAgainButton;
    public TMP_Text pauseResumeButtonText;
    public Texture2D pauseImageTexture;
    public Texture2D resumeImageTexture;
    public AudioSource audioSource;

    public DialogData[] dialogOptions; // Array of dialog options
    public Button[] topicButtons;
    private int dialogIndex = 0;
    private int sentenceIndex = 0;
    private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    private bool isPaused = false;
    private bool isTyping = false;

    void Start()
    {
        pauseResumeButton.onClick.AddListener(TogglePauseResume);
        playAgainButton.onClick.AddListener(PlayAgain);
        for (int i = 0; i < topicButtons.Length; i++)
        {
            int index = i; // Capture the correct value of i
            topicButtons[i].onClick.AddListener(() => PlayDialog(index));
        }


    }

    public void PlayDialog(int index)
    {
        learnMenuPanel.SetActive(false);
        contentDisplayPanel.SetActive(true);
        dialogIndex = index;
        sentenceIndex = 0;
        textDisplay.text = "";

        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.clip = dialogOptions[dialogIndex].audioClip;
        dialogueTopicText.text = dialogOptions[dialogIndex].dialogueName;

        dialogueTopicImage.sprite = dialogOptions[dialogIndex].topicImage;

        isPaused = false;
        pauseResumeButtonText.text = "Pause";
        pauseResumeButton.image.sprite = Sprite.Create(pauseImageTexture, new Rect(0, 0, pauseImageTexture.width, pauseImageTexture.height), new Vector2(0.5f, 0.5f));


        audioSource.Play();

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in dialogOptions[dialogIndex].sentences[sentenceIndex].ToCharArray())
        {
            while (isPaused) yield return null;

            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        yield return new WaitForSeconds(2f);
        NextSentence();
        //continueButton.SetActive(true);
        //audioSource.Pause();
    }


    public void OnContinueButtonClick()
    {
        audioSource.UnPause();
        NextSentence();
        continueButton.SetActive(false);
    }

    void Update()
    {
        if (!isTyping && textDisplay.text == dialogOptions[dialogIndex].sentences[sentenceIndex])
        {
            // Continue Button waits for manual click
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);

        if (sentenceIndex < dialogOptions[dialogIndex].sentences.Length - 1)
        {
            sentenceIndex++;
            textDisplay.text = "";
            typingCoroutine = StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            audioSource.Stop();
        }
    }

    void TogglePauseResume()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            audioSource.Pause();
            pauseResumeButtonText.text = "Resume";
            pauseResumeButton.image.sprite = Sprite.Create(resumeImageTexture, new Rect(0, 0, resumeImageTexture.width, resumeImageTexture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            audioSource.UnPause();
            pauseResumeButtonText.text = "Pause";
            pauseResumeButton.image.sprite = Sprite.Create(pauseImageTexture, new Rect(0, 0, pauseImageTexture.width, pauseImageTexture.height), new Vector2(0.5f, 0.5f));
        }

    }

    public void PlayAgain()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();


        isPaused = false;
        pauseResumeButtonText.text = "Pause";
        pauseResumeButton.image.sprite = Sprite.Create(pauseImageTexture, new Rect(0, 0, pauseImageTexture.width, pauseImageTexture.height), new Vector2(0.5f, 0.5f));

        sentenceIndex = 0;
        textDisplay.text = "";

        audioSource.clip = dialogOptions[dialogIndex].audioClip;
        audioSource.time = 0;
        audioSource.Play();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(Type());
    }

    public void ShowLearnMenuPage()
    {
        contentDisplayPanel.SetActive(false);
        learnMenuPanel.SetActive(true);
    }

}



[System.Serializable]
public class DialogData
{
    public string dialogueName;
    public Sprite topicImage;
    public string[] sentences;
    public AudioClip audioClip;
}
