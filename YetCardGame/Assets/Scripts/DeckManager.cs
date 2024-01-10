using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Unity.VisualScripting;
//using static Unity.VisualScripting.Metadata;
//using static UnityEditor.Experimental.GraphView.GraphView;

public enum Player
{
    Null,
    Player1,
    Player2
}
public enum Role
{
    Null,
    Attack,
    Defence
}
public class DeckManager : MonoBehaviour
{
    //[ReadOnly]
    public Player playerNumber;

    [SerializeField] RectTransform ParentInHandDeck;
    [SerializeField] RectTransform ParentOnTableDeck;

    //[ReadOnly]
    public List<Card> InHandDeck;
    //[ReadOnly]
    public List<Card> OnTableDeck;

    [SerializeField] int inHandCardOverLapUnits = 200;
    [SerializeField] int onTableCardOverLapUnits = 200;

    //[ReadOnly]
    public Role role;

    bool myTurn = false;
    bool played = false;

    private void Start()
    {
        ArrangeCardsInHandDeck();
    }
    public void Play(Player player)
    {
        if (player == Player.Player1)
        {
            MyTurn(true);
            if (role == Role.Attack)
            {
                GameplayUIManager.Instance.EnableAttackScreen(true);
            }
            else if (role == Role.Defence)
            {
                GameplayUIManager.Instance.EnableDefenceScreen(true);
            }
        }
        else if (player == Player.Player2)
        {
            MyTurn(true);

            GameplayUIManager.Instance.EnableEndTurnButton(false);

            GetComponent<EnemyAI>().Play();
        }
    }
    public void  PlayCard(Card card) //Called from enemy AI
    {
        for (int i = 0; i < InHandDeck.Count; i++)
        {
            if (InHandDeck[i] == card)
            {
                InHandDeck[i].PlayCard();
            }
        }
    }
    public bool IfCardPlayable(Card card)
    {
        #region Checking for attack card restrictions
        if (OnTableDeck.Count == 0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < OnTableDeck.Count; i++)
            {
                if (card.GetCardName() == OnTableDeck[i].GetCardName())
                {
                    return true;
                }
                if (card.GetCardElement() == OnTableDeck[i].GetCardElement())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
    public void EndTurn()
    {
        played = true;
        MyTurn(false);
        //if(role == Role.Attack)
        //{
        //    CalculateAttackAmount();
        //}
        //else if(role == Role.Defence)
        //{
        //    CalculateDefenceAmount();
        //}
        GameplayManager.Instance.EndTurn();
    }
    public bool MyTurn()
    {
        return myTurn;
    }
    public void MyTurn(bool b)
    {
        myTurn = b;
    }
    public int CalculateAttackAmount()
    {
        int totalAttack = 0;
        for (int i = 0; i < OnTableDeck.Count; i++)
        {
            totalAttack += OnTableDeck[i].AttackNumber;
        }
        return totalAttack;
    }
    public int CalculateDefenceAmount()
    {
        int totalDefence = 0;
        for (int i = 0; i < OnTableDeck.Count; i++)
        {
            totalDefence += OnTableDeck[i].DefenceNumber;
        }
        return totalDefence;
    }
    public void AddCardInHandDeck(Card card)
    {
        if (playerNumber == Player.Player1)
        {
            card.RotateInHand();
            card.AssignPlayer(Player.Player1, this);
        }
        else if (playerNumber == Player.Player2)
        {
            card.RotateUpsideDown();
            card.AssignPlayer(Player.Player2, this);
        }

        card.deck = Deck.InHandDeck;
        InHandDeck.Add(card);

        ArrangeCardsInHandDeck();
    }
    public void AddCardOnTableDeck(Card card)
    {
        if (playerNumber == Player.Player1)
        {
            //card.RotateInHand();
            //card.AssignPlayer(Player.Player1);
        }
        else if (playerNumber == Player.Player2)
        {
            //card.RotateUpsideDown();
            //card.AssignPlayer(Player.Player2);
            card.RotateDownsideUp();
        }

        RemoveFromInHandDeck(card);
        card.deck = Deck.OnTableDeck;
        OnTableDeck.Add(card);

        ArrangeCardsInHandDeck();
        ArrangeCardsOnTable();
    }
    void RemoveFromInHandDeck(Card card)
    {
        for (int i = 0; i < InHandDeck.Count; i++)
        {
            if (card == InHandDeck[i])
            {
                InHandDeck.RemoveAt(i);
                Debug.Log("RemovedFrom the list");
            }
        }
    }
    
    void ArrangeCardsInHandDeck()
    {
        foreach(var c in InHandDeck)
        {
            c.transform.parent = ParentInHandDeck;
        }

        if (InHandDeck.Count != 0)
        {
            if (InHandDeck.Count == 1)
            {
                Vector2 pos = new Vector2(0, 0);
                //InHandDeck[0].transform.parent = ParentInHandDeck;
                InHandDeck[0].SetTargetPosition(pos);
            }
            else if (InHandDeck.Count % 2 == 0)
            {
                int middle = InHandDeck.Count / 2;
                //InHandDeck[middle].transform.parent = ParentInHandDeck;

                Vector2 middlePos = new Vector2(inHandCardOverLapUnits / 2, 0);
                InHandDeck[middle].SetTargetPosition(middlePos);

                for (int i = middle - 1; i >= 0; i--) //From 0 to middle is the right side of the deck
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2((-(middle - i) * inHandCardOverLapUnits) - (-inHandCardOverLapUnits / 2), 0);
                    InHandDeck[i].SetTargetPosition(pos);
                }
                for (int i = InHandDeck.Count - 1; i > middle; i--) //From middle to up is the left side of the deck
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2((((i + 1) - middle) * inHandCardOverLapUnits) - (inHandCardOverLapUnits / 2), 0);
                    InHandDeck[i].SetTargetPosition(pos);
                }
            }
            else
            {
                int middle = InHandDeck.Count / 2;
                //InHandDeck[middle].transform.parent = ParentInHandDeck;

                Vector2 middlePos = new Vector2(0, 0);
                InHandDeck[middle].SetTargetPosition(middlePos);

                for (int i = middle - 1; i >= 0; i--)
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2(-(middle - i) * inHandCardOverLapUnits, 0);
                    //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    InHandDeck[i].SetTargetPosition(pos);
                }
                for (int i = InHandDeck.Count - 1; i > middle; i--)
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2((i - middle) * inHandCardOverLapUnits, 0);
                    //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    InHandDeck[i].SetTargetPosition(pos);
                }
            }
        }
    }
    void ArrangeCardsOnTable()
    {
        foreach (var c in OnTableDeck)
        {
            c.transform.parent = ParentOnTableDeck;
        }

        if (OnTableDeck.Count != 0)
        {
            if (OnTableDeck.Count == 1)
            {
                Vector2 pos = new Vector2(0, 0);
                //InHandDeck[0].transform.parent = ParentInHandDeck;
                OnTableDeck[0].SetTargetPosition(pos);
            }
            else if (OnTableDeck.Count % 2 == 0)
            {
                int middle = OnTableDeck.Count / 2;
                //InHandDeck[middle].transform.parent = ParentInHandDeck;

                Vector2 middlePos = new Vector2(onTableCardOverLapUnits / 2, 0);
                OnTableDeck[middle].SetTargetPosition(middlePos);

                for (int i = middle - 1; i >= 0; i--) //From 0 to middle is the right side of the deck
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2((-(middle - i) * onTableCardOverLapUnits) - (-onTableCardOverLapUnits / 2), 0);
                    OnTableDeck[i].SetTargetPosition(pos);
                }
                for (int i = OnTableDeck.Count - 1; i > middle; i--) //From middle to up is the left side of the deck
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2((((i + 1) - middle) * onTableCardOverLapUnits) - (onTableCardOverLapUnits / 2), 0);
                    OnTableDeck[i].SetTargetPosition(pos);
                }
            }
            else
            {
                int middle = OnTableDeck.Count / 2;
                //InHandDeck[middle].transform.parent = ParentInHandDeck;

                Vector2 middlePos = new Vector2(0, 0);
                OnTableDeck[middle].SetTargetPosition(middlePos);

                for (int i = middle - 1; i >= 0; i--)
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2(-(middle - i) * onTableCardOverLapUnits, 0);
                    //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    OnTableDeck[i].SetTargetPosition(pos);
                }
                for (int i = OnTableDeck.Count - 1; i > middle; i--)
                {
                    //InHandDeck[i].transform.parent = ParentInHandDeck;
                    Vector2 pos = new Vector2((i - middle) * onTableCardOverLapUnits, 0);
                    //   InHandDeck[i].gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                    OnTableDeck[i].SetTargetPosition(pos);
                }
            }
        }
    }
    public void ZoomCard(Card card)
    {
        ArrangeCardsInHandDeck();

        StartCoroutine(Zoom(card));
    }
    IEnumerator Zoom(Card card)
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < InHandDeck.Count; i++)
        {
            if (InHandDeck[i] == card)
            {
                for (int j = 0; j < i; j++)
                {
                    InHandDeck[j].SetTargetPosition(new Vector2(InHandDeck[j].transform.localPosition.x + 125, 0));
                    InHandDeck[j].Zoom(false);
                }
                for (int k = i + 1; k < InHandDeck.Count; k++)
                {
                    InHandDeck[k].SetTargetPosition(new Vector2(InHandDeck[k].transform.localPosition.x - 125, 0));
                    InHandDeck[k].Zoom(false);
                }

                break;
            }
        }
    }
    public bool IfPlayed()
    {
        return played;
    }
    public void ResetRound()
    {
        played = false;

        for (int i = 0; i < OnTableDeck.Count; i++)
        {
            Destroy(OnTableDeck[i].gameObject);
        }

        OnTableDeck.Clear();
    }
}
