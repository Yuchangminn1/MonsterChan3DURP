using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
    }
}
