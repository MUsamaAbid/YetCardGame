using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateManager : MonoBehaviour
{
    [SerializeField] int frameRate = 30;

    void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
