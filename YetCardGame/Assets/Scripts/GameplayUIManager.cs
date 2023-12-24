using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    #region Instance
    public static GameplayUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] GameObject AttackScreen;
    [SerializeField] GameObject DefendScreen;

    [SerializeField] GameObject Player1WinScreen;
    [SerializeField] GameObject Player2WinScreen;

    [SerializeField] GameObject SummaryScreen;
    [SerializeField] Text Player1Wins;
    [SerializeField] Text Player2Wins;

    [SerializeField] Text Player1WinsText;
    [SerializeField] Text Player2WinsText;

    [SerializeField] Button EndTurnButton;

    public void EnableAttackScreen(bool b)
    {
        AttackScreen.SetActive(b);
    }
    public void EnableDefenceScreen(bool b)
    {
        DefendScreen.SetActive(b);
    }
    public void EnablePlayer1WinScreen(bool b)
    {
        Player1WinScreen.SetActive(b);
    }
    public void EnablePlayer2WinScreen(bool b)
    {
        Player2WinScreen.SetActive(b);
    }
    public void UpdateWinningScore()
    {
        Player1WinsText.text = PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerOneWins, 0).ToString();
        Player2WinsText.text = PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerTwoWins, 0).ToString();
    }
    public void ShowSummaryScreen()
    {
        Player1Wins.text = "Player 1 wins: " + PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerOneWins, 0).ToString();
        Player2Wins.text = "Player 2 wins: " + PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerTwoWins, 0).ToString();

        SummaryScreen.SetActive(true);
    }
    public void EnableEndTurnButton(bool b)
    {
        EndTurnButton.interactable = b;
    }
}
