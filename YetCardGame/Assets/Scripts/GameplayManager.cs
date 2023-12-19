using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public void Toss()
    {
        int ran = UnityEngine.Random.Range(0, 100);
        if(ran > 50)
        {
            //player 2 attacker
            //player 1 defender
        }
        else
        {
            //player 1 attacker
            //player 2 defender
        }
    }
}
