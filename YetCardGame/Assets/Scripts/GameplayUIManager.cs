using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    #region Instance
    public static GameplayUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] GameObject AttackScreen;
    [SerializeField] GameObject DefendScreen;

    public void EnableAttackScreen(bool b)
    {
        AttackScreen.SetActive(b);
    }
    public void EnableDefenceScreen(bool b)
    {
        DefendScreen.SetActive(b);
    }
}
