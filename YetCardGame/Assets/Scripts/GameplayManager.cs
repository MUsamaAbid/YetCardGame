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
        if (Player1.IfPlayed() && Player2.IfPlayed())
        {
            CalculateResults();
            NextRound();
        }
        else
        {
            Debug.Log("!Player1: " + Player1.IfPlayed());
            Debug.Log("!Player2: " + Player2.IfPlayed());
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
                Debug.Log("Player1 won");
            }
            else
            {
                Debug.Log("Player 2 won");
            }
        }
        else
        {
            if(Player1.CalculateDefenceAmount() > Player2.CalculateAttackAmount())
            {
                Debug.Log("player 1 won");
            }
            else
            {
                Debug.Log("Player 2 won");
            }
        }
    }
    public void NextRound()
    {
        if(currentRound == Round.One || currentRound == Round.Two)
        {
            //Add to round
            if(currentRound == Round.One)
            {
                currentRound = Round.Two;
            }
            else if(currentRound == Round.Two)
            {
                currentRound = Round.Three;
            }

            //Swapping player and roles
            if(currentPlayer == Player.Player1)
            {

            }
            else if(currentPlayer == Player.Player2)
            {

            }
        }
    }
}
