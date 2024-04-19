using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private bool _gameStarted;
    private WordManager _wordManager;
    private readonly List<KeyCode> _keyCodes = new List<KeyCode>
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
        KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
        KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z
    };

    public int _currentCharacter;
    public int _currentWord;
    public string result;

    [SerializeField] private GameObject[] allGameContents;
    [SerializeField] private Word[] words;
    public int _letterAmount;

    [Header("Words Animation")] public Animator animator; 
    public GameObject wordAnimator;

    [Header("Letter Background")] public Color32 greenBackground, orangeBackground, neutralBackground, transparentColor;

    [Header("Game Over Panels")] public Animator panelAnimator;

    public AudioManager audioManager;
    
    
    public void StartGame()
    {
        FindActiveGameContent();
        _wordManager = GetComponent<WordManager>();
        _gameStarted = true;
    }

    void FindActiveGameContent()
    {
        foreach (var content in allGameContents)
        {
            if (content.activeSelf)
            {
                words = content.GetComponentsInChildren<Word>();
                wordAnimator = content.GetComponent<WordContent>().wordAnimator;
                animator = wordAnimator.GetComponent<Animator>();
            }
        }

        _letterAmount = WordManager.LetterAmount + 4;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame(false);
        }
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DeleteLetter();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                EnterWord();
            }
            else
            {
                foreach (var key in _keyCodes)
                {
                    if (Input.GetKeyDown(key))
                    {
                        string letter = key.ToString();
                        EnterCharacter(letter);
                        break;
                    }
                }
            }
        }
    }

    void EnterCharacter(string letter)
    {
        if (_currentCharacter < _letterAmount)
        {
            audioManager.ClickSound();
            wordAnimator.transform.position = words[_currentWord].letters[_currentCharacter].transform.position;
            animator.Play("fillLetter", 0,0);
            words[_currentWord].letters[_currentCharacter].text.text = letter;
            _currentCharacter++;
            var insert = result.Insert(result.Length, letter);
            result = insert;
        }
    }

    public void DeleteLetter()
    {
        if (_gameStarted)
        {
            if (_currentCharacter > 0)
            {
                _currentCharacter--;
                words[_currentWord].letters[_currentCharacter].text.text = " ";
                var remove = result.Remove(result.Length - 1, 1);
                result = remove;
                letters.RemoveAt(letters.Count - 1);
                audioManager.ClickSound();
            }
        }
    }


    [SerializeField] private GameObject deleteButton, clearButton;
    public void EnterWord()
    {
        if (_gameStarted && result.Length == _letterAmount)
        {
            audioManager.ClickSound();
            foreach (string str in _wordManager.allWords)
            {
                if (str.Contains(result))
                {
                    HighLightResults();
                    break;
                }
                else
                {
                    words[_currentWord].wordAnimator.Play("invalidWord", 0, 0);
                }
            }
        }
    }

    
    public TextMeshProUGUI resultWord;

    public List<char> firstWordArrToList;
    public List<char> secondWordArrToList;
    void HighLightResults()
    {
        char[] firstWordArr = result.ToCharArray(0, _currentCharacter);
        char[] secondWordArr = WordManager.RandomWord.ToCharArray(0, _currentCharacter);

        int greenLetterCount = 0;
        
        foreach (var buttonImage in backgroundButtonImage)
        {
            for (int i = 0; i < letters.Count; i++)
            {
                if (!secondWordArr.Contains(firstWordArr[i]) && !firstWordArr[i].Equals(secondWordArr[i]))
                {
                    if (words[_currentWord].letters[i].text.text == buttonImage.gameObject.name)
                    {
                        buttonImage.color = neutralBackground;
                    }
                }
            }
        }
        
        for (int i = 0; i < _currentCharacter; i++)
        {
            if (firstWordArr[i].Equals(secondWordArr[i]))
            {
                greenLetterCount++;
                //secondWordArr[Array.IndexOf(secondWordArr, firstWordArr[i])] = ' ';
                secondWordArr[i] = ' ';
                firstWordArr[i] = ' ';
                words[_currentWord].letters[i].backgroundImage.color = greenBackground;
                foreach (var buttonImage in backgroundButtonImage)
                {
                    if (words[_currentWord].letters[i].text.text == buttonImage.gameObject.name)
                    {
                        buttonImage.color = greenBackground;
                    }
                }
            }
        }
        
        for (int i = 0;  i< _currentCharacter; i++)
        {
            if (!firstWordArr[i].Equals(secondWordArr[i]))
            {
                if (secondWordArr.Contains(firstWordArr[i])) //&& words[_currentWord].letters[i].backgroundImage.color != greenBackground)
                {
                    words[_currentWord].letters[i].backgroundImage.color = orangeBackground;
                    //secondWordArr[i] = ' ';
                    //Array.IndexOf(secondWordArr, firstWordArr[i]);
                    secondWordArr[Array.IndexOf(secondWordArr, firstWordArr[i])] = ' ';
                    
                    foreach (var buttonImage in backgroundButtonImage)
                    {
                        if (words[_currentWord].letters[i].text.text == buttonImage.gameObject.name)
                        {
                            buttonImage.color = orangeBackground;
                        }
                    }
                }
            }
            // else if (!secondWordArr.Contains(firstWordArr[i]) && !firstWordArr[i].Equals(secondWordArr[i]))
            // {
            //     words[_currentWord].letters[i].backgroundImage.color = neutralBackground;
            // }
            
        }
        secondWordArrToList = secondWordArr.ToList();
        firstWordArrToList = firstWordArr.ToList();

        // for (int i = 0; i < _currentCharacter; i++)
        // {
        //     
        // }
        words[_currentWord].wordAnimator.Play("fillWord", 0,0);
        if (greenLetterCount == _currentCharacter)
        {
            panelAnimator.Play("winPrompt");
            //Debug.Log("WINWINWINWINWINWINWIN");
            audioManager.WinSound();
            _gameStarted = false;
        }
        else if (_currentWord == _letterAmount)
        {
            panelAnimator.Play("losePrompt");
            resultWord.text = _wordManager.currentWord;
            _gameStarted = false;
        }
        else
        {
            _currentWord++;
            _currentCharacter = 0;
            result = "";
        }
        
        letters.Clear();
    }
    
    public List<Image> backgroundButtonImage;
    public List<string> letters;
    public void EnterKey(string letter)
    {
        EnterCharacter(letter);
        if (letters.Count < _letterAmount)
        {
            letters.Add(letter);
        }
    }
    public void RestartGame(bool isMainMenu)
    {
        foreach (var word in words)
        {
            word.wordAnimator.Play("originalBackgroundLetterState");
            foreach (var letter in word.letters)
            {
                letter.text.text = "";
                letter.backgroundImage.color = neutralBackground;
            }
        }

        foreach (var button in backgroundButtonImage)
        {
            button.color = transparentColor;
        }
        panelAnimator.Play("noPrompt");
        result = "";
        _currentWord = 0;
        _currentCharacter = 0;
        if (!isMainMenu)
        {
            _wordManager.GetRandomWord();
        }
        _gameStarted = true;
        letters.Clear();
    }
}
