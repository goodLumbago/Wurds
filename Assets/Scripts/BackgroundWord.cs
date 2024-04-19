using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundWord : MonoBehaviour
{
    public GameObject textPrefab;
    private Animator _animator;
    private TextMeshProUGUI _textString;

    private string[] words;

    void OnEnable()
    {
        words = LoadWords();
        StartCoroutine(InstantiatePrefab());
    }
    
    void DeployPrefab()
    {
        var randomPos = new Vector2(Random.Range(-700, 700), Random.Range(-1200, 1200));
        var go = Instantiate(textPrefab, transform.position, quaternion.identity);
        go.transform.SetParent(transform);
        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.localPosition = randomPos;
        rectTransform.localScale = new Vector3(1, 1, 1);
        
        _textString = textPrefab.GetComponentInChildren<TextMeshProUGUI>();
        _textString.text = words[Random.Range(0, words.Length)];
    }
    IEnumerator InstantiatePrefab()
    {
        
        DeployPrefab();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(InstantiatePrefab());
    }
    public static string[] LoadWords()
    {
        string text = Resources.Load<TextAsset>("words1").text;
        return text.Split(", ");
    }
    
    
    
}
