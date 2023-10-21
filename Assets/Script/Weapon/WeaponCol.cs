using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class WeaponCol : MonoBehaviour
{
    MeshCollider meshCollider;
    Boss boss;
    [SerializeField] GameObject[] particleSystem;
    void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = false;

    }
    void Start()
    {
        UIScript.instance.playerParticle[2] = transform.GetComponent<ParticleSystem>();
    }
    public void WeaponColOn()
    {
        if(meshCollider!= null)
        {
            meshCollider.enabled = true;
        }
        else
        {
            Debug.Log("meshCollider is null");
        }
    }
    public void WeaponColOff()
    {
        if (meshCollider != null)
        {
            meshCollider.enabled = false;
        }
        else
        {
            Debug.Log("meshCollider is null");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BossHitBox")
        {
            Boss boss = other.GetComponentInParent<Boss>();
            if (boss != null)
            {
                boss.Hit(20,10);
                //foreach (var particle in particleSystem)
                //{
                //    if (particle.GetComponent<ParticleSystem>().IsAlive())
                //        continue;
                    
                //    particle.transform.position = other.ClosestPoint(transform.position);
                //    particle.GetComponent<ParticleSystem>().Play();
                //    break;
                //}

            }
            

        }
    }
}
