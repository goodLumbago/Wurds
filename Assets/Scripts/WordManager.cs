using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [SerializeField] private TextAsset words;

    public string[] stringWords;
    public string[] allWords;
    public static string RandomWord;

    
    public static int LetterAmount;
    private InputManager _inputManager;
    public AudioManager audioManager;
    
    public string testString = "VASE";

    [SerializeField] private List<GameObject> wordContents;
    private Animator _animator;
    public bool isTesting;
    void Awake()
    {
        _inputManager = GetComponent<InputManager>();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void LoadDictionary()
    {
        _animator.Play("menuTransitionPlay");
        EnableWordContent(LetterAmount);
        //stringWords = words.text.Split(' ');
        stringWords = LoadWords();
        allWords = LoadAllWords();

        for (int i = 0; i < stringWords.Length; i++)
        {
            stringWords[i] = stringWords[i].ToUpper();
        }
        for (int i = 0; i < allWords.Length; i++)
        {
            allWords[i] = allWords[i].ToUpper();
        }
    }

    public GameObject menuUI;
    public void StartGame(int difficulty)
    {
        ChangeLetterAmount(difficulty);
        LoadDictionary();
        _inputManager.StartGame();
        if (!isTesting)
        {
            GetRandomWord();
        }
        else
        {
            RandomWord = testString;
            currentWord = RandomWord;
        }
        audioManager.ButtonClick();
    }

    public void DisableMenu()
    {
        if (menuUI.activeSelf)
        {
            menuUI.SetActive(false);
        }
        else
        {
            menuUI.SetActive(true);
            leaveGamePrompt.SetActive(false);
            _inputManager.RestartGame(true);
        }
        audioManager.ButtonClick();
    }

    public void MainMenu()
    {
        _animator.Play("menuTransitionPlay");
        //menuUI.SetActive(true);
        audioManager.ButtonClick();
    }

    public GameObject leaveGamePrompt;
    public void LeaveGame()
    {
        leaveGamePrompt.SetActive(true);
        audioManager.ButtonClick();
    }

    public void YesLeaveGame()
    {
        MainMenu();
        audioManager.ButtonClick();
    }

    public void DontLeaveGame()
    {
        leaveGamePrompt.SetActive(false);
        audioManager.ButtonClick();
    }
    public string currentWord;
    

    void EnableWordContent(int lm)
    {
        for (int i = 0; i < wordContents.Count; i++)
        {
            if (i == lm)
            {
                wordContents[i].SetActive(true);
            }
            else
            {
                wordContents[i].SetActive(false);
            }
        }
    }

    static void ChangeLetterAmount(int lm)
    {
        LetterAmount = lm;
    }

    public void GetRandomWord()
    {
        RandomWord = stringWords[Random.Range(0, stringWords.Length)];
        currentWord = RandomWord;
    }
    public static string[] LoadWords()
    {
        string text = Resources.Load<TextAsset>("words" + LetterAmount).text;
        return text.Split(", ");
    }
    public static string[] LoadAllWords()
    {
        string text = Resources.Load<TextAsset>("allwords" + LetterAmount).text;
        return text.Split(" ");
    }

    [SerializeField] private GameObject tutorialObject;
    public void Tutorial()
    {
        if (tutorialObject.activeSelf)
        {
            tutorialObject.SetActive(false);
        }
        else
        {
            tutorialObject.SetActive(true);
        }
        audioManager.ButtonClick();
    }
}
