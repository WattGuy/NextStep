using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVSync : MonoBehaviour
{

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }

}
