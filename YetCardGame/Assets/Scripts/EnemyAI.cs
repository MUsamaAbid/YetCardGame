using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AILevel
{
    Null,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Eleven,
    Twelve,
    Thirteen,
    Fourteen
}

public class EnemyAI : MonoBehaviour
{
    DeckManager Player1;
    DeckManager Player2;

    List<Card> AttackCards;
    List<Card> DefenceCards;

    Role role;

    private void Start()
    {
        Player1 = GameplayCardManager.Instance.ReturnPlayer(Player.Player1);
        Player2 = GameplayCardManager.Instance.ReturnPlayer(Player.Player2);
    }
    private void Update()
    {
        //SortAttackCards();
        //SortDefenceCards();
    }
    public void Play()
    {
        role = Player2.role;

        if (role == Role.Attack)
        {
            SortAttackCards();
        }
        else if (role == Role.Defence)
        {
            SortDefenceCards();
        }
        StartCoroutine(PlayCard());
    }
    IEnumerator PlayCard()
    {
        yield return new WaitForSeconds(1f);

        if (role == Role.Attack)
        {
            Player2.PlayCard(AttackCards[0]);
        }
        else if (role == Role.Defence)
        {
            Player2.PlayCard(DefenceCards[0]);
        }
        Invoke("EndTurn", 2f);
    }
    void EndTurn()
    {
        Player2.EndTurn();
    }
    void SortAttackCards()
    {
        AttackCards = Player2.InHandDeck.GetRange(0, Player2.InHandDeck.Count);

        // Use a lambda expression to sort the list based on AttackNumber
        AttackCards.Sort((card1, card2) => card1.AttackNumber.CompareTo(card2.AttackNumber));

        // Print the sorted list
        //for (int i = 0; i < AttackCards.Count; i++)
        //{
        //    Debug.Log(AttackCards[i].name + ":" + AttackCards[i].AttackNumber);
        //}
    }
    void SortDefenceCards()
    {
        DefenceCards = Player2.InHandDeck.GetRange(0, Player2.InHandDeck.Count);

        // Use a lambda expression to sort the list based on AttackNumber
        DefenceCards.Sort((card1, card2) => card1.DefenceNumber.CompareTo(card2.DefenceNumber));

        // Print the sorted list
        //for (int i = 0; i < DefenceCards.Count; i++)
        //{
        //    Debug.Log(DefenceCards[i].name + ":" + DefenceCards[i].DefenceNumber);
        //}
    }
    int ReturnPlayerAttack()
    {
        List<Card> PlayerOnTable = Player1.OnTableDeck;

        int totalAttack = 0;

        foreach (var c in PlayerOnTable)
        {
            totalAttack += c.AttackNumber;
        }
        return totalAttack;
    }
    int ReturnPlayerDefence()
    {
        List<Card> PlayerOnTable = Player1.OnTableDeck;

        int totalDefence = 0;

        foreach (var c in PlayerOnTable)
        {
            totalDefence += c.DefenceNumber;
        }
        return totalDefence;
    }
}
