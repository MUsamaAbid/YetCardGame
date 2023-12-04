using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("For Element card")]
    [SerializeField] CardElement cardElement;
    [SerializeField] CardName cardName;

    [Header("For Fate Cards")]
    [SerializeField] FateCard fateCard;
    [SerializeField] Curses curses;
    [SerializeField] Spells spells;
}
