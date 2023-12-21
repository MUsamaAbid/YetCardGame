using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Round
{
    Null,
    One,
    Two,
    Three
}
public class GameplayManager : MonoBehaviour
{
    #region Instance
    public static GameplayManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    DeckManager Player1;
    DeckManager Player2;

    Player currentPlayer;
    Round currentRound;

    private void Start()
    {
        Player1 = GameplayCardManager.Instance.ReturnPlayer(Player.Player1);
        Player2 = GameplayCardManager.Instance.ReturnPlayer(Player.Player2);

        Invoke("Toss", 5f);
    }
    public void Toss()
    {
        currentRound = Round.One;

        int ran = Random.Range(0, 100);
        if(ran > 50)
        {
            Player1.role = Role.Defence;
            Player2.role = Role.Attack;

            currentPlayer = Player.Player2;

            Player2.Play(Player.Player2);
        }
        else
        {
            Player1.role = Role.Attack;
            Player2.role = Role.Defence;

            currentPlayer = Player.Player1;

            Player1.Play(Player.Player1);

            GameplayUIManager.Instance.EnableAttackScreen(true);
        }
        
    }
    public void EndTurn()
    {
        Debug.Log("Round: " + currentRound);
        Debug.Log("Player1 Played: " + Player1.IfPlayed());
        Debug.Log("Player2 Played: " + Player2.IfPlayed());

        if (Player1.IfPlayed() && Player2.IfPlayed())
        {
            CalculateResults();
            if (currentRound == Round.One || currentRound == Round.Two)
            {
                //Add to round
                if (currentRound == Round.One)
                {
                    currentRound = Round.Two;
                }
                else if (currentRound == Round.Two)
                {
                    currentRound = Round.Three;
                }
                StartCoroutine(StartNextRound());
            }
            else if(currentRound == Round.Three)
            {
                Debug.Log("Game finished");
                GameplayUIManager.Instance.ShowSummaryScreen();
            }
        }
        else
        {
            //Debug.Log("!Player1: " + Player1.IfPlayed());
            //Debug.Log("!Player2: " + Player2.IfPlayed());
            //Swapping player and roles
            if (currentPlayer == Player.Player1)
            {
                if (Player1.role == Role.Attack)
                {
                    Player2.role = Role.Defence;
                }
                else if (Player1.role == Role.Defence)
                {
                    Player2.role = Role.Attack;
                }
                currentPlayer = Player.Player2;

                //Player1.role = Role.Null;

                Player2.Play(Player.Player2);
            }
            else if (currentPlayer == Player.Player2)
            {
                if (Player2.role == Role.Attack)
                {
                    Player1.role = Role.Defence;
                }
                else if (Player2.role == Role.Defence)
                {
                    Player1.role = Role.Attack;
                }
                currentPlayer = Player.Player1;

                //Player2.role = Role.Null;

                Player1.Play(Player.Player1);
            }
        }
    }
    public void CalculateResults()
    {
        if(Player1.role == Role.Attack)
        {
            if(Player1.CalculateAttackAmount() > Player2.CalculateDefenceAmount())
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player1, 2);
                GameplayUIManager.Instance.EnablePlayer1WinScreen(true);

                string str = PrefsHandler.Instance.PlayerOneWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);
                Debug.Log("Player 1 wins: " + PlayerPrefs.GetInt(str, 0));
            }
            else
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player2, 2);
                GameplayUIManager.Instance.EnablePlayer2WinScreen(true);

                string str = PrefsHandler.Instance.PlayerTwoWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);
            }
        }
        else
        {
            if(Player1.CalculateDefenceAmount() > Player2.CalculateAttackAmount())
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player1, 2);
                GameplayUIManager.Instance.EnablePlayer1WinScreen(true);

                string str = PrefsHandler.Instance.PlayerOneWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);

                Debug.Log("Player 1 wins: " + PlayerPrefs.GetInt(str, 0));
            }
            else
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player2, 2);
                GameplayUIManager.Instance.EnablePlayer2WinScreen(true);

                string str = PrefsHandler.Instance.PlayerTwoWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);
            }
        }
    }
    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(5f);
        NextRound();
    }
    public void NextRound()
    {
        Player1.ResetRound();
        Player2.ResetRound();

        //Swapping player and roles
        if (currentPlayer == Player.Player1) //Second player is the defence one
        {
            Player1.role = Role.Attack;
            Player2.role = Role.Defence;

            currentPlayer = Player.Player1;

            Player1.Play(Player.Player1);

            GameplayUIManager.Instance.EnableAttackScreen(true);
        }
        else if (currentPlayer == Player.Player2) //Second player is the defence one
        {
            Player1.role = Role.Defence;
            Player2.role = Role.Attack;

            currentPlayer = Player.Player2;

            Player2.Play(Player.Player2);
        }
    }
}
