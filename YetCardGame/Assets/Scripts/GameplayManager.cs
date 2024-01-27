using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        currentRound = Round.One;

        Invoke("Toss", 5f);
    }
    public void Toss()
    {
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
        //Debug.Log("Round: " + currentRound);
        //Debug.Log("Player1 Played: " + Player1.IfPlayed());
        //Debug.Log("Player2 Played: " + Player2.IfPlayed());

        if (Player1.IfPlayed() && Player2.IfPlayed())
        {
            GameplayUIManager.Instance.EnableEndTurnButton(false);

            CalculateResults();
            Debug.Log("Rounds:" + currentRound);
            if (currentRound == Round.One || currentRound == Round.Two)
            {
                //Add to round
                if (currentRound == Round.One)
                {
                    currentRound = Round.Two;
                    StartCoroutine(StartNextRound());
                }
                else if (currentRound == Round.Two)
                {
                    if(PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerOneWins, 0) == 2
                        || PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerTwoWins, 0) == 2)
                    {
                        if(PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerOneWins, 0) == 2)
                        {
                            PlayerPrefs.SetInt(PrefsHandler.Instance.currentLevel, SceneManager.GetActiveScene().buildIndex);
                            GameplayUIManager.Instance.EnableWinScreen();
                        }
                        else
                        {
                            GameplayUIManager.Instance.EnableLoseScreen();
                        }

                        GameplayUIManager.Instance.ShowSummaryScreen();
                    }
                    else
                    {
                        currentRound = Round.Three;

                        Player1.ResetRound();
                        Player2.ResetRound();

                        Invoke("Toss", 5f);
                    }
                }
            }
            else if(currentRound == Round.Three)
            {
                Debug.Log("Game finished");

                if (PlayerPrefs.GetInt(PrefsHandler.Instance.PlayerOneWins, 0) == 2)
                {
                    PlayerPrefs.SetInt(PrefsHandler.Instance.currentLevel, SceneManager.GetActiveScene().buildIndex);
                    GameplayUIManager.Instance.EnableWinScreen();
                }
                else
                {
                    GameplayUIManager.Instance.EnableLoseScreen();
                }

                GameplayUIManager.Instance.ShowSummaryScreen();
            }
        }
        else
        {
            //Swapping player and roles
            if (currentPlayer == Player.Player1)
            {
                GameplayUIManager.Instance.EnableEndTurnButton(false);

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
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player1, 2, CardType.Army);
                GameplayUIManager.Instance.EnablePlayer1WinScreen(true);

                string str = PrefsHandler.Instance.PlayerOneWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);
                Debug.Log("Player 1 wins: " + PlayerPrefs.GetInt(str, 0));
            }
            else
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player2, 2, CardType.Army);
                GameplayUIManager.Instance.EnablePlayer2WinScreen(true);

                string str = PrefsHandler.Instance.PlayerTwoWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);
            }
        }
        else
        {
            if(Player1.CalculateDefenceAmount() > Player2.CalculateAttackAmount())
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player1, 2, CardType.Army);
                GameplayUIManager.Instance.EnablePlayer1WinScreen(true);

                string str = PrefsHandler.Instance.PlayerOneWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);

                Debug.Log("Player 1 wins: " + PlayerPrefs.GetInt(str, 0));
            }
            else
            {
                GameplayCardManager.Instance.DistributeMoreCards(Player.Player2, 2, CardType.Army);
                GameplayUIManager.Instance.EnablePlayer2WinScreen(true);

                string str = PrefsHandler.Instance.PlayerTwoWins;
                PlayerPrefs.SetInt(str, PlayerPrefs.GetInt(str, 0) + 1);
            }
        }
        GameplayUIManager.Instance.UpdateWinningScore();
    }
    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(1f);

        Player1.ResetRound();
        Player2.ResetRound();

        yield return new WaitForSeconds(3f);

        NextRound();
    }
    public void NextRound()
    {
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
    public void RemoveCard(Player player, int numberOfCards)
    {
        if (Player1.playerNumber == player)
        {
            if (numberOfCards > Player1.InHandDeck.Count)
            {
                Player1.DeleteInHandCards();
            }
            else
            {
                int ran = Random.Range(0, Player1.InHandDeck.Count - numberOfCards);
                for (int i = 0; i < numberOfCards; i++)
                {
                    Player1.RemoveFromInHandDeck(ran + i);
                }
            }
        }
        else if (Player2.playerNumber == player)
        {
            if (Player2.playerNumber == player)
            {
                if (numberOfCards > Player2.InHandDeck.Count)
                {
                    Player2.DeleteInHandCards();
                }
                else
                {
                    int ran = Random.Range(0, Player2.InHandDeck.Count - numberOfCards);
                    for (int i = 0; i < numberOfCards; i++)
                    {
                        Player2.RemoveFromInHandDeck(ran + i);
                    }
                }
            }
        }
    }
    public void RemoveHalfBlue(Player player)
    {
        if(Player1.playerNumber == player)
        {
            RemovingHalfBlue(Player1);
        }
        else if(Player2.playerNumber == player)
        {
            RemovingHalfBlue(Player2);
        }
    }
    void RemovingHalfBlue(DeckManager player)
    {
        int count = 0;
        for (int i = 0; i < player.InHandDeck.Count; i++)
        {
            if (player.InHandDeck[i].GetCardType() == CardType.Army)
            {
                if (player.InHandDeck[i].GetCardElement() == CardElement.Water)
                {
                    count++;

                }
            }
        }
        if (count > 1)
            count = count / 2;

        if (count != 0)
        {
            int k = 0;
            for (int j = 0; j < player.InHandDeck.Count; j++)
            {
                if (player.InHandDeck[j].GetCardType() == CardType.Army)
                {
                    if (player.InHandDeck[j].GetCardElement() == CardElement.Water)
                    {
                        player.RemoveFromInHandDeck(player.InHandDeck[j]);
                        player.ArrangeCardsInHandDeck();

                        k++;
                        if (k >= count)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    public void RemoveHalfGreen(Player player)
    {
        if (Player1.playerNumber == player)
        {
            RemovingHalfGreen(Player1);
        }
        else if (Player2.playerNumber == player)
        {
            RemovingHalfGreen(Player2);
        }
    }
    void RemovingHalfGreen(DeckManager player)
    {
        int count = 0;
        for (int i = 0; i < player.InHandDeck.Count; i++)
        {
            if (player.InHandDeck[i].GetCardType() == CardType.Army)
            {
                if (player.InHandDeck[i].GetCardElement() == CardElement.Earth)
                {
                    count++;

                }
            }
        }
        if (count > 1)
            count = count / 2;

        if (count != 0)
        {
            int k = 0;
            for (int j = 0; j < player.InHandDeck.Count; j++)
            {
                if (player.InHandDeck[j].GetCardType() == CardType.Army)
                {
                    if (player.InHandDeck[j].GetCardElement() == CardElement.Earth)
                    {
                        player.RemoveFromInHandDeck(player.InHandDeck[j]);
                        player.ArrangeCardsInHandDeck();

                        k++;
                        if (k >= count)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    public void RemoveHalfRed(Player player)
    {
        if (Player1.playerNumber == player)
        {
            RemovingHalfRed(Player1);
        }
        else if (Player2.playerNumber == player)
        {
            RemovingHalfRed(Player2);
        }
    }
    void RemovingHalfRed(DeckManager player)
    {
        int count = 0;
        for (int i = 0; i < player.InHandDeck.Count; i++)
        {
            if (player.InHandDeck[i].GetCardType() == CardType.Army)
            {
                if (player.InHandDeck[i].GetCardElement() == CardElement.Water)
                {
                    count++;

                }
            }
        }
        if (count > 1)
            count = count / 2;

        if (count != 0)
        {
            int k = 0;
            for (int j = 0; j < player.InHandDeck.Count; j++)
            {
                if (player.InHandDeck[j].GetCardType() == CardType.Army)
                {
                    if (player.InHandDeck[j].GetCardElement() == CardElement.Water)
                    {
                        player.RemoveFromInHandDeck(player.InHandDeck[j]);
                        player.ArrangeCardsInHandDeck();

                        k++;
                        if (k >= count)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    public void RemoveHalfBlack(Player player)
    {
        if (Player1.playerNumber == player)
        {
            RemovingHalfBlack(Player1);
        }
        else if (Player2.playerNumber == player)
        {
            RemovingHalfBlack(Player2);
        }
    }
    void RemovingHalfBlack(DeckManager player)
    {
        int count = 0;
        for (int i = 0; i < player.InHandDeck.Count; i++)
        {
            if (player.InHandDeck[i].GetCardType() == CardType.Army)
            {
                if (player.InHandDeck[i].GetCardElement() == CardElement.Air)
                {
                    count++;

                }
            }
        }
        if (count > 1)
            count = count / 2;

        if (count != 0)
        {
            int k = 0;
            for (int j = 0; j < player.InHandDeck.Count; j++)
            {
                if (player.InHandDeck[j].GetCardType() == CardType.Army)
                {
                    if (player.InHandDeck[j].GetCardElement() == CardElement.Air)
                    {
                        player.RemoveFromInHandDeck(player.InHandDeck[j]);
                        player.ArrangeCardsInHandDeck();

                        k++;
                        if (k >= count)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadScene(int sceneNumber)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNumber);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        GameplayUIManager.Instance.EnablePauseScreen(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        GameplayUIManager.Instance.EnablePauseScreen(false);
    }
    public void ExitApplication()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
