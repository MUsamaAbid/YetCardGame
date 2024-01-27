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

    bool noSpell = false;

    bool noCurse = false;
    bool noCurseBlack = false;
    bool noCurseRed = false;
    bool noCurseBlue = false;
    bool noCurseGreen = false;

    bool combineAnyColor = false;

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
        if (card.GetCardType() == CardType.Army)
        {
            if (OnTableDeck.Count == 0 || combineAnyColor)
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
        }
        #endregion

        else if (card.GetCardType() == CardType.Fate)
        {
            if(card.GetFateCard() == FateCard.Curses)
            {
                return TakeCurseAction(card);
            }
            else if(card.GetFateCard() == FateCard.Spells)
            {
                return TakeSpellAction(card);
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    bool TakeCurseAction(Card card)
    {
        if (noCurse) return false;

        switch (card.GetCurseName())
        {
            case Curses.BribeDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Army);
                return true;
                break;

            case Curses.LighteningDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveCard(Player.Player2, 2);
                else
                    GameplayManager.Instance.RemoveCard(Player.Player1, 2);
                return true;
                break;

            case Curses.BlueDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveHalfBlue(Player.Player2);
                else
                    GameplayManager.Instance.RemoveHalfBlue(Player.Player1);
                return true;
                break;

            case Curses.GreenDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveHalfGreen(Player.Player2);
                else
                    GameplayManager.Instance.RemoveHalfGreen(Player.Player1);
                return true;
                break;

            case Curses.RedDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveHalfRed(Player.Player2);
                else
                    GameplayManager.Instance.RemoveHalfRed(Player.Player1);
                return true;
                break;

            case Curses.BlackDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveHalfBlack(Player.Player2);
                else
                    GameplayManager.Instance.RemoveHalfBlack(Player.Player1);
                return true;
                break;

            case Curses.CharonDescription:
                //End spell
                return true;
                break;

            case Curses.FearDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveCard(Player.Player2, 1);
                else
                    GameplayManager.Instance.RemoveCard(Player.Player1, 1);
                return true;
                break;

            case Curses.IIIDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 2, CardType.Army);
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Fate);
                return true;
                break;

            case Curses.RocDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveCard(Player.Player2, 4);
                else
                    GameplayManager.Instance.RemoveCard(Player.Player1, 4);
                return true;
                break;

            case Curses.ThunderDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.RemoveCard(Player.Player2, 3);
                else
                    GameplayManager.Instance.RemoveCard(Player.Player1, 3);
                return true;
                break;

            case Curses.LyreDescription:
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.NoSpell(Player.Player2);
                else
                    GameplayManager.Instance.NoSpell(Player.Player1);
                return true;
                break;

            case Curses.FoolDescription:
                //Take spell
                GameplayCardManager.Instance.DistributeMoreFateCards(playerNumber, 1, FateCard.Spells);
                return true;
                break;

            case Curses.GoneDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 3, CardType.Army);
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Fate);
                return true;
                break;

            case Curses.SkullDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Army);
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Fate);
                return true;
                break;

            default:
                Debug.Log("Default option or unknown value selected");
                return true;
                break;
        }
    }
    bool TakeSpellAction(Card card)
    {
        if (noSpell) return false;

        switch (card.GetSpellsName())
        {
            case Spells.BannerDescription:
                //No curses
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.NoCurses(Player.Player2);
                else
                    GameplayManager.Instance.NoCurses(Player.Player1);
                return true;
                break;

            case Spells.BatsDescription:
                //No curses black
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.NoCursesBlack(Player.Player2);
                else
                    GameplayManager.Instance.NoCursesBlack(Player.Player1);
                return true;
                break;

            case Spells.CaughtDescription:
                //Take curse
                GameplayCardManager.Instance.DistributeMoreFateCards(playerNumber, 1, FateCard.Curses);
                return true;
                break;

            case Spells.DrinkDescription:
                IncreaseHandAmount(3);
                return true;
                break;

            case Spells.FeastDescription:
                IncreaseHandAmount(5);
                return true;
                break;

            case Spells.FrogsDescription:
                //No curses red
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.NoCursesRed(Player.Player2);
                else
                    GameplayManager.Instance.NoCursesRed(Player.Player1);
                return true;
                break;

            case Spells.HealthDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Army);
                return true;
                break;

            case Spells.JusticeDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 3, CardType.Army);
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Fate);
                return true;
                break;

            case Spells.LibertyDescription:
                //End curse
                return true;
                break;

            case Spells.MiracleDescription:
                IncreaseHandAmount(4);
                return true;
                break;

            case Spells.MusicDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 2, CardType.Army);
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Fate);
                return true;
                break;

            case Spells.SeahorseDescription:
                //No curses blue
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.NoCurseBlue(Player.Player2);
                else
                    GameplayManager.Instance.NoCurseBlue(Player.Player1);
                return true;
                break;

            case Spells.TortoiseDescription:
                //No curses green
                if (playerNumber == Player.Player1)
                    GameplayManager.Instance.NoCurseGreen(Player.Player2);
                else
                    GameplayManager.Instance.NoCurseGreen(Player.Player1);
                return true;
                break;

            case Spells.UnityDescription:
                //Combine any color
                CombineAnyColor(true);
                return true;
                break;

            case Spells.WealthDescription:
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Army);
                GameplayCardManager.Instance.DistributeMoreCards(playerNumber, 1, CardType.Fate);
                return true;
                break;

            default:
                Debug.Log("Default option or unknown value selected");
                return true;
                break;
        }
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
    public void RemoveFromInHandDeck(Card card)
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
    public void RemoveFromInHandDeck(int number)
    {
        if (number > InHandDeck.Count)
        {
            for (int i = 0; i < InHandDeck.Count; i++)
            {
                Destroy(OnTableDeck[i]);
                ArrangeCardsInHandDeck();
            }
        }
        else
        {
            int ran = Random.Range(0, InHandDeck.Count - number);

            for (int i = 0; i < number; i++)
            {
                Destroy(OnTableDeck[ran + i]);
                ArrangeCardsInHandDeck();
            }
        }
        ArrangeCardsInHandDeck();
    }
    //public void DeleteFromOnTableDeck(Card card)
    //{
    //    for (int i = 0; i < OnTableDeck.Count; i++)
    //    {
    //        if (card == OnTableDeck[i])
    //        {
    //            Destroy(OnTableDeck[i]);
    //        }
    //    }
    //    ArrangeCardsOnTable();
    //}
    
    public void DeleteInHandCards()
    {
        for (int i = 0; i < InHandDeck.Count; i++)
        {
            Destroy(InHandDeck[i]);
        }
    }
    public void ArrangeCardsInHandDeck()
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
                InHandDeck[i].SetTargetPosition(new Vector2(InHandDeck[i].transform.localPosition.x, 60));

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
    public void IncreaseHandAmount(int amount)
    {
        foreach(var c in InHandDeck)
        {
            if(c.GetCardType() == CardType.Army)
            {
                c.AttackNumber += amount;
                c.DefenceNumber += amount;
            }
        }
    }
    public void NoSpell(bool b)
    {
        noSpell = b;
    }
    public void NoCurse(bool b)
    {
        noCurse = b;
    }
    public void NoCurseBlack(bool b)
    {
        noCurseBlack = b;
    }
    public void NoCurseRed(bool b)
    {
        noCurseRed = b;
    }
    public void NoCurseBlue(bool b)
    {
        noCurseBlue = b;
    }
    public void NoCurseGreen(bool b)
    {
        noCurseGreen = b;
    }
    public void CombineAnyColor(bool b)
    {
        combineAnyColor = b;
    }
    public void ResetRound()
    {
        played = false;

        for (int i = 0; i < OnTableDeck.Count; i++)
        {
            Destroy(OnTableDeck[i].gameObject);
        }

        //Resetting card actions
        NoSpell(false);
        NoCurse(false);
        NoCurseBlack(false);
        NoCurseBlue(false);
        NoCurseRed(false);
        NoCurseGreen(false);
        CombineAnyColor(false);
        ReAssignProperties();

        OnTableDeck.Clear();
    }
    public void ReAssignProperties()
    {
        foreach (var c in InHandDeck)
        {
            c.AssignProperties();
        }
    }
}
