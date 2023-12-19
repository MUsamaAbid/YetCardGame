using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    DeckManager Player1;
    DeckManager Player2;

    private void Start()
    {
        Player1 = GameplayCardManager.Instance.ReturnPlayer(Player.Player1);
        Player2 = GameplayCardManager.Instance.ReturnPlayer(Player.Player2);

        Invoke("Toss", 5f);
    }
    public void Toss()
    {
        int ran = UnityEngine.Random.Range(0, 100);
        if(ran > 50)
        {
            Player1.role = Role.Defence;
            Player2.role = Role.Attack;
        }
        else
        {
            Player1.role = Role.Attack;
            Player2.role = Role.Defence;
        }
        Player2.Play(Player.Player2);
    }
}
