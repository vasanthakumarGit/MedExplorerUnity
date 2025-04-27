using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager: MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public GameObject continueButton;
    public Button pauseResumeButton;
    public TMP_Text pauseResumeButtonText;
    public AudioSource audioSource;

    public DialogData[] dialogOptions; // Array of dialog options
    private int dialogIndex = 0;
    private int sentenceIndex = 0;
    private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    private bool isPaused = false;
    private bool isTyping = false;

    void Start()
    {
        pauseResumeButton.onClick.AddListener(TogglePauseResume);
    }

    public void PlayDialog(int index)
    {
        dialogIndex = index;
        sentenceIndex = 0;
        textDisplay.text = "";

        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.clip = dialogOptions[dialogIndex].audioClip;
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
        }
        else
        {
            audioSource.UnPause();
            pauseResumeButtonText.text = "Pause";
        }

    }
}



[System.Serializable]
public class DialogData
{
    public string dialogueName;
    public string[] sentences;
    public AudioClip audioClip;
}
