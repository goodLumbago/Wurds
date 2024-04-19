using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour
{
    public Letter[] letters;
    [HideInInspector] public Animator wordAnimator;
    void Awake()
    {
        letters = GetComponentsInChildren<Letter>();
        Debug.Log(letters.Length);
    }

    void Start()
    {
        wordAnimator = GetComponent<Animator>();
        foreach (var character in letters)
        {
            character.text.text = " ";
        }
    }
}
