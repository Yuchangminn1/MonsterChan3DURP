using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    Animator animator;
    Enemy enemy;
    public int AttackDamage = 5;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        AttackDamage = enemy.attackDagame ;
        animator = GetComponent<Animator>();
    }

    protected virtual void StateReset()
    {
        animator.SetInteger("State", 0);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //    {
    //        Debug.Log("Hit");
    //        collision.transform.GetComponent<PlayerController>().Hit(AttackDamage);    
    //    }
    //}
}
