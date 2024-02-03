using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsHandler : MonoBehaviour
{
    #region Instance
    public static PrefsHandler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public string PlayerOneWins = "PlayerOneWins";
    public string PlayerTwoWins = "PlayerTwoWins";

    public string currentLevel = "CurrentLevel";

    public string currentBoss = "CurrentBoss";

    private void Start()
    {
        ResetPrefs();
    }
    private void ResetPrefs()
    {
        PlayerPrefs.SetInt(PlayerOneWins, 0);
        PlayerPrefs.SetInt(PlayerTwoWins, 0);
    }
}
