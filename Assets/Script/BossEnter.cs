using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnter : MonoBehaviour
{
    void Start()
    {
        UIScript.instance.UIOnOff(0, false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIScript.instance.UIOnOff(0, true);
            gameObject.SetActive(false);
        }
    }
}
