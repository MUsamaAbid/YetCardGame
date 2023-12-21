using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnable : MonoBehaviour
{
    [SerializeField] GameObject g;
    [SerializeField] float DestroyAfter;

    private void OnEnable()
    {
        Invoke("DestroyGameObject", DestroyAfter);
    }
    void DestroyGameObject()
    {
        if (!g)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(g);
        }
    }
}
