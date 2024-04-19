using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    [SerializeField] private AudioClip clickSound;
    public void ClickSound()
    {
        _audioSource.PlayOneShot(clickSound);
    }

    [SerializeField] private AudioClip winSound;
    public void WinSound()
    {
        _audioSource.PlayOneShot(winSound);
    }
    [SerializeField] private AudioClip menuButtonClick;
    public void ButtonClick()
    {
        _audioSource.PlayOneShot(menuButtonClick);
    }

    public Button soundButton;
    public void TurnSound()
    {
        if (_audioSource.volume != 1)
        {
            _audioSource.volume = 1;
            soundButton.image.color = Color.white;
        }
        else
        {
            _audioSource.volume = 0;
            soundButton.image.color = Color.grey;
        }
    }
}
