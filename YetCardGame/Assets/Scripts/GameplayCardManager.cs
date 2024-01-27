using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameplayCardManager : MonoBehaviour
{
    #region Instance
    public static GameplayCardManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] GameObject MainDeckParent;

    [SerializeField] List<Card> MainCardDeck;
    [SerializeField] List<Card> FateCardDeck;

    [SerializeField] List<Card> Player1Deck;
    [SerializeField] List<Card> Player2Deck;

    [SerializeField] DeckManager Player1;
    [SerializeField] DeckManager Player2;

    private void Start()
    {
        AddCardsToMainDeck();
        ShuffleArmyCards();
        ShuffleFateCards();
        StartCoroutine(DistributeCards(6, CardType.Army));
    }
    void AddCardsToMainDeck()
    {
        foreach (var cards in MainCardDeck)
        {
            cards.gameObject.transform.parent = MainDeckParent.transform;
        }
        foreach(var cards in FateCardDeck)
        {
            cards.gameObject.transform.parent = MainDeckParent.transform;
        }
        //MainCardDeck.Reverse();
    }
    private void ShuffleArmyCards()
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
    private void ShuffleFateCards()
    {
        int cardCount = FateCardDeck.Count;

        // Iterate through the list in reverse order
        for (int i = cardCount - 1; i > 0; i--)
        {
            // Generate a random index within the remaining unshuffled cards
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            // Swap the current card with the randomly selected card
            Card temp = FateCardDeck[i];
            FateCardDeck[i] = FateCardDeck[randomIndex];
            FateCardDeck[randomIndex] = temp;
        }

        // Now, the 'cards' list is shuffled
    }
    IEnumerator DistributeCards(int cardAmount, CardType cardType)
    {
        //Shuffle cards by swapping there places.
        //Pick a random number and distribute from there
        if (cardType == CardType.Army)
        {
            int r = UnityEngine.Random.Range(0, MainCardDeck.Count - ((2 * cardAmount) - 1));
            for (int i = r; i < (r + (2 * cardAmount)); i++)
            {
                {
                    Player1.AddCardInHandDeck(MainCardDeck[i]);
                    MainCardDeck.RemoveAt(i);
                }
                i++;
                {
                    Player2.AddCardInHandDeck(MainCardDeck[i]);
                    MainCardDeck.RemoveAt(i);
                }
                yield return new WaitForSeconds(0.5f);
            }
            StartCoroutine(DistributeCards(2, CardType.Fate));
        }
        else if(cardType == CardType.Fate)
        {
            int r = UnityEngine.Random.Range(0, FateCardDeck.Count - ((2 * cardAmount) - 1));
            for (int i = r; i < (r + (2 * cardAmount)); i++)
            {
                {
                    Player1.AddCardInHandDeck(FateCardDeck[i]);
                    FateCardDeck.RemoveAt(i);
                }
                i++;
                {
                    Player2.AddCardInHandDeck(FateCardDeck[i]);
                    FateCardDeck.RemoveAt(i);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public void DistributeMoreCards(Player player, int cardAmount, CardType cardType)
    {
        StartCoroutine(DistributeCards(player, cardAmount, cardType));
    }
    IEnumerator DistributeCards(Player player, int cardAmount, CardType cardType)
    {
        if (cardType == CardType.Army)
        {
            //Shuffle cards by swapping there places.
            //Pick a random number and distribute from there
            int r = UnityEngine.Random.Range(0, MainCardDeck.Count - cardAmount - 1);
            for (int i = r; i < (r + cardAmount); i++)
            {
                if (player == Player.Player1)
                {
                    Player1.AddCardInHandDeck(MainCardDeck[i]);
                    MainCardDeck.RemoveAt(i);
                }
                else if (player == Player.Player2)
                {
                    Player2.AddCardInHandDeck(MainCardDeck[i]);
                    MainCardDeck.RemoveAt(i);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if(cardType == CardType.Fate)
        {
            //Shuffle cards by swapping there places.
            //Pick a random number and distribute from there
            int r = UnityEngine.Random.Range(0, FateCardDeck.Count - cardAmount - 1);
            for (int i = r; i < (r + cardAmount); i++)
            {
                if (player == Player.Player1)
                {
                    Player1.AddCardInHandDeck(FateCardDeck[i]);
                    FateCardDeck.RemoveAt(i);
                }
                else if (player == Player.Player2)
                {
                    Player2.AddCardInHandDeck(FateCardDeck[i]);
                    FateCardDeck.RemoveAt(i);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public void DistributeMoreFateCards(Player player, int cardAmount, FateCard cardType)
    {
        StartCoroutine(DistributeFateCards(player, cardAmount, cardType));
    }
    IEnumerator DistributeFateCards(Player player, int cardAmount, FateCard cardType)
    {
        if (cardType == FateCard.Curses)
        {
            //Shuffle cards by swapping there places.
            //Pick a random number and distribute from there
            for (int i = 0; i < FateCardDeck.Count; i++)
            {
                if (FateCardDeck[i].GetFateCard() == FateCard.Curses)
                {
                    if (player == Player.Player1)
                    {
                        Player1.AddCardInHandDeck(FateCardDeck[i]);
                        FateCardDeck.RemoveAt(i);
                    }
                    else if (player == Player.Player2)
                    {
                        Player2.AddCardInHandDeck(FateCardDeck[i]);
                        FateCardDeck.RemoveAt(i);
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        else if (cardType == FateCard.Spells)
        {
            //Shuffle cards by swapping there places.
            //Pick a random number and distribute from there
            for (int i = 0; i < FateCardDeck.Count; i++)
            {
                if (FateCardDeck[i].GetFateCard() == FateCard.Spells)
                {
                    if (player == Player.Player1)
                    {
                        Player1.AddCardInHandDeck(FateCardDeck[i]);
                        FateCardDeck.RemoveAt(i);
                    }
                    else if (player == Player.Player2)
                    {
                        Player2.AddCardInHandDeck(FateCardDeck[i]);
                        FateCardDeck.RemoveAt(i);
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void AddToPlayer1Deck(Card card)
    {
        Player1Deck.Add(card);
        Player1.AddCardInHandDeck(card);
    }
    public DeckManager ReturnPlayer(Player player)
    {
        if(player == Player.Player1)
        {
            return Player1;
        }
        else
        {
            return Player2;
        }

    }
    //Shuffle deck
    List<T> ShuffleList<T>(List<T> list)
    {
        List<T> shuffledList = new List<T>(list);
        int n = shuffledList.Count;
        System.Random random = new System.Random();

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = shuffledList[i];
            shuffledList[i] = shuffledList[j];
            shuffledList[j] = temp;
        }

        return shuffledList;
    }
}