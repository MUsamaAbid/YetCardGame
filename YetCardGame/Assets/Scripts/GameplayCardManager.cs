using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameplayCardManager : MonoBehaviour
{
    [SerializeField] GameObject MainDeckParent;

    [SerializeField] List<Card> MainCardDeck;
    [SerializeField] List<Card> Player1Deck;
    [SerializeField] List<Card> Player2Deck;

    [SerializeField] DeckManager Player1;
    [SerializeField] DeckManager Player2;

    private void Start()
    {
        AddCardsToMainDeck();
        ShuffleCards();
        DistributeCards(Player.Player1, 1);
        DistributeCards(Player.Player2, 1);
    }
    void AddCardsToMainDeck()
    {
        foreach (var cards in MainCardDeck)
        {
            cards.gameObject.transform.parent = MainDeckParent.transform;
        }
    }
    private void ShuffleCards()
    {
        int cardCount = MainCardDeck.Count;

        // Iterate through the list in reverse order
        for (int i = cardCount - 1; i > 0; i--)
        {
            // Generate a random index within the remaining unshuffled cards
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            // Swap the current card with the randomly selected card
            Card temp = MainCardDeck[i];
            MainCardDeck[i] = MainCardDeck[randomIndex];
            MainCardDeck[randomIndex] = temp;
        }

        // Now, the 'cards' list is shuffled
    }
    void DistributeCards(Player player, int cardAmount)
    {
        //Shuffle cards by swapping there places.
        //Pick a random number and distribute from there
        int r = UnityEngine.Random.Range(0, MainCardDeck.Count - cardAmount - 1);
        for (int i = r; i < (r + cardAmount); i++)
        {
            if(player == Player.Player1)
            {
                Player1.AddCardInHandDeck(MainCardDeck[i]);
                MainCardDeck.RemoveAt(i);
            }
            else if (player == Player.Player2)
            {
                Player2.AddCardInHandDeck(MainCardDeck[i]);
                MainCardDeck.RemoveAt(i);
            }
        }

    }
    public void AddToPlayer1Deck(Card card)
    {
        Player1Deck.Add(card);
        Player1.AddCardInHandDeck(card);
    }
}