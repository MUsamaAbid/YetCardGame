using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterEnable : MonoBehaviour
{
    [SerializeField] GameObject g;
    [SerializeField] float DisableAfter;

    private void OnEnable()
    {
        Invoke("DisableGameobject", DisableAfter);
    }
    void DisableGameobject()
    {
        if (!g)
        {
            gameObject.SetActive(false);
        }
        else
        {
            g.SetActive(false);
        }
    }
}
