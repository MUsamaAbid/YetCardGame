using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
public enum Deck
{
    Null,
    InHandDeck,
    OnTableDeck
}
public class Card : MonoBehaviour
{
    [SerializeField] CardType cardType;

    [Header("For Army card")]
    [SerializeField] CardElement cardElement;
    [SerializeField] CardName cardName;

    [Header("For Fate Cards")]
    [SerializeField] FateCard fateCard;
    [SerializeField] Curses curses;
    [SerializeField] Spells spells;

    [SerializeField] GameObject ImageGameobject;
    [SerializeField] Sprite BackSide;

    [SerializeField] GameObject GeneratedBacksideImage;

    public DeckManager deckManager;
    public Deck deck;

    Player player;

    [Header("Card Properties")]
//    [ReadOnly]
    public int AttackNumber;
  //  [ReadOnly]
    public int DefenceNumber;

    public bool cardCounts;

    Vector2 Targetpos;
    bool MoveToTargetPos = false;
    int speed = 1000;

    bool zoomed = false;
    private void Start()
    {
        AssignProperties();
        gameObject.GetComponent<Button>().onClick.AddListener(OnCardDown);

        transform.rotation = Quaternion.Euler(0, 180, 0);
        if (ImageGameobject && BackSide)
        {
            GenerateBackside();
        }
    }
    public void OnCardDown()
    {
        if (player == Player.Player1 && !deckManager.IfPlayed() && deckManager.MyTurn())
        {
            if (deck == Deck.InHandDeck && zoomed)
            {
                Zoom(false);

                PlayCard();
            }
            else
            {
                deckManager.ZoomCard(this);

                Zoom(true);
            }
        }
    }

    public void PlayCard()
    {
        if (player == Player.Player2)//Wont come here because restriction is on OnCardDown()
        {
            deckManager.AddCardOnTableDeck(this);

            //return;
        }
        else if (player == Player.Player1)
        {
            //Old way
            //deckManager.AddCardOnTableDeck(this);

            //if (deckManager.role == Role.Attack)
            //{
            //    deckManager.EndTurn();
            //}
            //else
            //{
            //    GameplayUIManager.Instance.EnableEndTurnButton(true);
            //}

            //New way
            if (deckManager.role == Role.Attack)
            {
                if (deckManager.IfCardPlayable(this))
                {
                    deckManager.AddCardOnTableDeck(this);

                    GameplayUIManager.Instance.EnableEndTurnButton(true);
                }
                else
                {
                    //Generate some wrong error or whatever message with some instrucitons
                }
            }
            else
            {
                deckManager.AddCardOnTableDeck(this);

                GameplayUIManager.Instance.EnableEndTurnButton(true);
            }
        }
    }

    public void AssignPlayer(Player p, DeckManager d)
    {
        player = p;
        deckManager = d;
    }
    void AssignProperties()
    {
        AttackNumber = CardEnums.instance.ReturnAttack(cardName);
        DefenceNumber = CardEnums.instance.ReturnDefence(cardName);
    }
    public void RotateInHand()
    {
        GetComponent<RotateObject>().RotateInHand(2f, ObjectType.Card);
        //GenerateBackside();
    }
    public void RotateUpsideDown()
    {
        GetComponent<RotateObject>().RotateUpsideDown(4f, ObjectType.Card);
        //GenerateBackside();
    }
    public void RotateDownsideUp()
    {
        GetComponent<RotateObject>().RotateDownsideUp(4f, ObjectType.Card);
    }
    public void GenerateBackside()
    {
        GeneratedBacksideImage = Instantiate(ImageGameobject, transform);
        GeneratedBacksideImage.GetComponent<Image>().sprite = BackSide;
        GeneratedBacksideImage.GetComponent<Image>().SetNativeSize();
        ExtendAnchorsToCanvas(GeneratedBacksideImage.GetComponent<RectTransform>());
    }
    public void DeleteBackside()
    {
        if (GeneratedBacksideImage)
        {
            Debug.Log("Generated: " + GeneratedBacksideImage.name);
            Destroy(GeneratedBacksideImage);
            Debug.Log("Backside deleted");
        }
    }

    public void ExtendAnchorsToCanvas(RectTransform rectTransform)
    {
        if (rectTransform != null && rectTransform.parent != null)
        {
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            if (parentRectTransform != null)
            {
                // Set the size to match the parent
                rectTransform.sizeDelta = parentRectTransform.sizeDelta;

                // Reset the anchored position (center it in the parent)
                rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                Debug.LogError("Parent does not have a RectTransform component!");
            }
        }
        else
        {
            Debug.LogError("RectTransform or parent is null!");
        }
    }
    public void SetTargetPosition(Vector2 Pos)
    {
        Targetpos = Pos;
        MoveToTargetPos = true;
    }
    private void Update()
    {
        if (MoveToTargetPos)
        {
            Vector2 currentAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            Vector2 targetAnchoredPosition = Targetpos;

            // Calculate the direction and distance to the target
            Vector2 direction = (targetAnchoredPosition - currentAnchoredPosition).normalized;
    //        float distance = Vector2.Distance(currentAnchoredPosition, targetAnchoredPosition);

            // Calculate the new anchored position based on the constant speed
            Vector2 newAnchoredPosition = currentAnchoredPosition + direction * speed * Time.deltaTime;

            // Ensure that we don't overshoot the target
            if (Vector2.Distance(newAnchoredPosition, targetAnchoredPosition) < speed * Time.deltaTime)
            {
                newAnchoredPosition = targetAnchoredPosition;
            }

            // Update the anchored position of the RectTransform
            GetComponent<RectTransform>().anchoredPosition = newAnchoredPosition;
        }
    }
    public void Zoom(bool b)
    {
        zoomed = b;
    }
    public void CardCounts(bool b)
    {
        cardCounts = b;
    }
    public bool CardCounts()
    {
        return cardCounts;
    }
    #region Return information
    public CardType GetCardType()
    {
        return cardType;
    }
    public CardElement GetCardElement()
    {
        return cardElement;
    }
    public CardName GetCardName()
    {
        return cardName;
    }
    public FateCard GetFateCard()
    {
        return fateCard;
    }
    public Curses GetCurseName()
    {
        return curses;
    }
    public Spells GetSpellsName()
    {
        return spells;
    }
    #endregion
}