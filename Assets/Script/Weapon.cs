using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject eqPos;
    void Start()
    {
        transform.parent = eqPos.transform;
        //transform.localPosition = Vector3.one;
        //transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
