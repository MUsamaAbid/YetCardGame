using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBackground : MonoBehaviour
{
    [SerializeField] GameObject[] locations;

    private void OnEnable()
    {
        int index = PlayerPrefs.GetInt(PrefsHandler.Instance.currentBoss, 0);
        if (index == 0 || index == 1)
        {
            locations[0].SetActive(true);
        }
        else
        {
            if (locations[index - 1])
            {
                locations[index - 1].SetActive(true);
            }
            else
            {
                locations[0].SetActive(true);
            }
        }
    }
}
