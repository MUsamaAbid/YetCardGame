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
        // Calculate the height of each portion
        //Old start
        //float portionWidth = (ParentRectTransform.rect.width + cardOverLapUnits) / InHandDeck.Count;

        //// Calculate the total width of the cards including the overlap distance
        //float totalWidth = InHandDeck.Count * portionWidth + (InHandDeck.Count - 1) * cardOverLapUnits;

        //// Calculate the starting position to center the cards horizontally
        //float startXPos = -totalWidth / 2f + portionWidth / 2f;

        //// Iterate through the number of cards and create and position them
        //for (int i = 0; i < InHandDeck.Count; i++)
        //{
        //    // Calculate the position for the current card, including the overlap
        //    //float xPos = startXPos + i * (portionWidth + cardOverLapUnits);
        //    float xPos = startXPos + i * (portionWidth + cardOverLapUnits);
        //    float yPos = ParentRectTransform.rect.height / 2f; // You can adjust this as needed

        //    // Set the position of the card
        //    InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);

        //    // You can customize this part to set the card's data or behavior as needed

        //}
        //Old end
        //float cardSpacing = 10f; // Set the desired spacing between cards

        //// Calculate the width of each portion (card + spacing)
        //float portionWidth = (ParentRectTransform.rect.width + cardSpacing) / InHandDeck.Count;

        //// Calculate the total width of the cards including the overlap distance
        //float totalWidth = InHandDeck.Count * portionWidth - cardSpacing;

        //// Calculate the starting position to center the cards horizontally
        //float startXPos = -totalWidth / 2f + portionWidth / 2f;

        //// Iterate through the number of cards and create and position them
        //for (int i = 0; i < InHandDeck.Count; i++)
        //{
        //    // Calculate the position for the current card, including the overlap
        //    float xPos = startXPos + i * portionWidth;

        //    // yPos is set to zero, as the cards are arranged along the horizontal axis
        //    float yPos = 0f;

        //    // Set the position of the card
        //    InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);

        //    // You can customize this part to set the card's data or behavior as needed
        //}


        /*//Way 2
        float cardWidth = InHandDeck[0].GetComponent<RectTransform>().rect.width;

        //InHandDeck[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((ParentRectTransform.rect.width / 2), 0);
        
        //Way 3
        for (int i = 0; i < InHandDeck.Count; i++)
        {
            // Adjust the position of the card based on its width and the desired spacing.
            float xPosition = i * (cardWidth + 10f); // 10f is the spacing between cards
            InHandDeck[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition , 0f);
        }
        */
        if (InHandDeck.Count != 0)
        {
            if (InHandDeck.Count == 1)
            {
                InHandDeck[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
            else if (InHandDeck.Count % 2 == 0)
            {
                int middle = InHandDeck.Count / 2;
                InHandDeck[middle].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(cardOverLapUnits / 2, 0);

                for (int i = middle - 1; i >= 0; i--) //From 0 to middle is the right side of the deck
                {
                    Vector2 pos = new Vector2((-(middle - i) * cardOverLapUnits) - (-cardOverLapUnits / 2), 0);
                    InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                }
                for (int i = InHandDeck.Count - 1; i > middle; i--) //From middle to up is the left side of the deck
                {
                    Vector2 pos = new Vector2((((i + 1) - middle) * cardOverLapUnits) - (cardOverLapUnits / 2), 0);
                    InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                }
            }
            else
            {
                int middle = InHandDeck.Count / 2;
                InHandDeck[middle].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                for (int i = middle - 1; i >= 0; i--)
                {
                    Vector2 pos = new Vector2(-(middle - i) * cardOverLapUnits, 0);
                    InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                }
                for (int i = InHandDeck.Count - 1; i > middle; i--)
                {
                    Vector2 pos = new Vector2((i - middle) * cardOverLapUnits, 0);
                    InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                }
            }
        }
    }
}
