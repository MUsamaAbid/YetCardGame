using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(1);
    }
    public void CloseApplication()
    {
        Application.Quit();
    }
}
