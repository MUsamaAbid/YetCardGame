using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCardManager : MonoBehaviour
{
    [SerializeField] Card[] MainCardDeck;
    [SerializeField] Card[] Player1Deck;
    [SerializeField] Card[] Player2Deck;

    [SerializeField] DeckManager Player1;
    [SerializeField] DeckManager Player2;
}