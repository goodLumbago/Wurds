using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimationFinish : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
