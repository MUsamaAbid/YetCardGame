using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public enum Player
{
    Null,
    Player1,
    Player2
}
public class DeckManager : MonoBehaviour
{
    [SerializeField] Player playerNumber;
    [SerializeField] RectTransform ParentRectTransform;

    [SerializeField] List<Card> InHandDeck;

    [SerializeField] int cardOverLapUnits = 200;

    [SerializeField] GameObject[] InHandDeckPlaces;

    private void Start()
    {
        ArrangeCardsInHandDeck();
    }

    public void AddCardToDeck(Card card)
    {
        InHandDeck.Add(card);
        ArrangeCardsInHandDeck();
    }

    void ArrangeCardsInHandDeck()
    {
        if (InHandDeck.Count != 0)
        {
            if (InHandDeck.Count == 1)
            {
                Vector2 pos = new Vector2(0, 0);
                InHandDeck[0].SetTargetPosition(pos);
            }
            else if (InHandDeck.Count % 2 == 0)
            {
                int middle = InHandDeck.Count / 2;
                Vector2 middlePos = new Vector2(cardOverLapUnits / 2, 0);
                InHandDeck[middle].SetTargetPosition(middlePos);

                for (int i = middle - 1; i >= 0; i--) //From 0 to middle is the right side of the deck
                {
                    //InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    Vector2 pos = new Vector2((-(middle - i) * cardOverLapUnits) - (-cardOverLapUnits / 2), 0);
                    InHandDeck[i].SetTargetPosition(pos);
                }
                for (int i = InHandDeck.Count - 1; i > middle; i--) //From middle to up is the left side of the deck
                {
                    Vector2 pos = new Vector2((((i + 1) - middle) * cardOverLapUnits) - (cardOverLapUnits / 2), 0);
                    //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    InHandDeck[i].SetTargetPosition(pos);
                }
            }
            else
            {
                int middle = InHandDeck.Count / 2;
                Vector2 middlePos = new Vector2(0, 0);
                InHandDeck[middle].SetTargetPosition(middlePos);

                for (int i = middle - 1; i >= 0; i--)
                {
                    Vector2 pos = new Vector2(-(middle - i) * cardOverLapUnits, 0);
                 //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    InHandDeck[i].SetTargetPosition(pos);
                }
                for (int i = InHandDeck.Count - 1; i > middle; i--)
                {
                    Vector2 pos = new Vector2((i - middle) * cardOverLapUnits, 0);
                 //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    InHandDeck[i].SetTargetPosition(pos);
                }
            }
        }
    }
}
