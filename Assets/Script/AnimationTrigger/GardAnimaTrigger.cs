using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardAnimaTrigger : MonoBehaviour
{
    [SerializeField] Transform gard;
    Animator animator;
    BoxCollider2D box;
    bool spawn = false;
    public int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
       // gard = GetComponentInParent<Transform>();
        animator = transform.GetComponent<Animator>();
        box = transform.GetComponentInParent<BoxCollider2D>();
    }

    public void EndSpawn()
    {
        spawn = false;
        gard.transform.position = new Vector2(-200f, -200f);
        animator.SetBool("Spawn", spawn);
    }
    public void Spawn(int _counter,Vector3 _spawnPos)
    {
        Debug.Log("º“»Ø«‘ _spawnPos = " + _spawnPos);
        spawn = true;
        gard.transform.position = _spawnPos;
        animator.SetInteger("Counter", _counter);
        animator.SetBool("Spawn", spawn);
    }

    
    void AttackOn()
    {
        box.enabled = true;
    }
    void AttackOff()
    {
        box.enabled = false;
    }
}
