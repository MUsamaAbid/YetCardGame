using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardElement
{
    Null,
    Fire,
    Air,
    Earth,
    Water,
}
public enum CardName
{
    Null,
    Amazon,
    Arrows,
    Ballista,
    Chariot,
    Elephant,
    Enyo,
    Eris,
    Guard,
    Hero,
    Horse,
    Hounds,
    Lion,
    Mask,
    Shields,
    Stone,
    Sword,
    Tower,
    Trident,
    Zealot,
    //Fire
    Bunyip,
    Sphinx,
    //Water
    Namaka,
    Lilith,
    //Air
    Minos,
    Caludia,
    //Earth
    Medusa,
    Harpy,
}
public enum FateCard
{
    Null,
    Curses,
    Spells
}
public enum Curses
{
    Null,
    BribeDescription,
    LighteningDescription,
    BlueDescription,
    GreenDescription,
    RedDescription,
    BlackDescription,
    CharonDescription,
    FearDescription,
    IIIDescription,
    RocDescription,
    ThunderDescription,
    LyreDescription,
    FoolDescription,
    GoneDescription,
    SkullDescription
}
public enum Spells
{
    Null,
    BannerDescription,
    BatsDescription,
    CaughtDescription,
    DrinkDescription,
    FeastDescription,
    FrogsDescription,
    HealthDescription,
    JusticeDescription,
    LibertyDescription,
    MiracleDescription,
    MusicDescription,
    SeahorseDescription,
    TortoiseDescription,
    UnityDescription,
    WealthDescription
}
public class CardEnums : MonoBehaviour
{
    public int ReturnAttack(CardName cardName)
    {
        switch (cardName)
        {
            case CardName.Amazon:
                return 7;
                break;

            case CardName.Arrows:
                return 7;
                break;

            case CardName.Ballista:
                return 15;
                break;

            case CardName.Chariot:
                return 6;
                break;

            case CardName.Elephant:
                return 12;
                break;

            case CardName.Enyo:
                return 5;
                break;

            case CardName.Eris:
                return 9;
                break;

            case CardName.Guard:
                return 6;
                break;

            case CardName.Hero:
                return 8;
                break;

            case CardName.Horse:
                return 4;
                break;

            case CardName.Hounds:
                return 2;
                break;

            case CardName.Lion:
                return 8;
                break;

            case CardName.Mask:
                return 0;
                break;

            case CardName.Shields:
                return 0;
                break;

            case CardName.Stone:
                return 2;
                break;

            case CardName.Sword:
                return 5;
                break;

            case CardName.Tower:
                return 0;
                break;

            case CardName.Trident:
                return 3;
                break;

            case CardName.Zealot:
                return 10;
                break;

                //Fire
            case CardName.Bunyip:
                return 28;
                break;

            case CardName.Sphinx:
                return 8;
                break;

            //Water

            case CardName.Namaka:
                return 20;
                break;

            case CardName.Lilith:
                return 15;
                break;

            //Air

            case CardName.Minos:
                return 20;
                break;

            case CardName.Caludia:
                return 15;
                break;

            //Earth

            case CardName.Medusa:
                return 30;
                break;

            case CardName.Harpy:
                return 5;
                break;

            default:
                return 0;
                break;
        }
        return 0;
    }
    public int ReturnDefence(CardName cardName)
    {
        switch (cardName)
        {
            case CardName.Amazon:
                return 6;
                break;

            case CardName.Arrows:
                return 2;
                break;

            case CardName.Ballista:
                return 2;
                break;

            case CardName.Chariot:
                return 6;
                break;

            case CardName.Elephant:
                return 15;
                break;

            case CardName.Enyo:
                return 8;
                break;

            case CardName.Eris:
                return 4;
                break;

            case CardName.Guard:
                return 10;
                break;

            case CardName.Hero:
                return 8;
                break;

            case CardName.Horse:
                return 4;
                break;

            case CardName.Hounds:
                return 3;
                break;

            case CardName.Lion:
                return 3;
                break;

            case CardName.Mask:
                return 2;
                break;

            case CardName.Shields:
                return 6;
                break;

            case CardName.Stone:
                return 1;
                break;

            case CardName.Sword:
                return 2;
                break;

            case CardName.Tower:
                return 20;
                break;

            case CardName.Trident:
                return 1;
                break;

            case CardName.Zealot:
                return 6;
                break;

            //Fire
            case CardName.Bunyip:
                return 12;
                break;

            case CardName.Sphinx:
                return 22;
                break;

            //Water

            case CardName.Namaka:
                return 10;
                break;

            case CardName.Lilith:
                return 25;
                break;

            //Air

            case CardName.Minos:
                return 20;
                break;

            case CardName.Caludia:
                return 15;
                break;

            //Earth

            case CardName.Medusa:
                return 10;
                break;

            case CardName.Harpy:
                return 25;
                break;

            default:
                return 0;
                break;
        }
        return 0;
    }
}
