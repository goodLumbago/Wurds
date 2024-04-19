using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI currentVersionText;
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 50;

        var currentVersion = Application.version;
        currentVersionText.text = currentVersion;
    }
}
