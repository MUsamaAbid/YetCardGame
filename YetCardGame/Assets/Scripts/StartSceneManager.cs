using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        if (PlayerPrefs.GetInt(PrefsHandler.Instance.currentLevel) == 6)
        {
            SceneManager.LoadScene(6);
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt(PrefsHandler.Instance.currentLevel) + 1);
        }
    }
    public void CloseApplication()
    {
        Application.Quit();
    }
}
