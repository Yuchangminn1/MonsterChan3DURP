using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationTrigger : MonoBehaviour
{
    Animator animator;
    [SerializeField] ParticleSystem[] fireBreathR;
    [SerializeField] ParticleSystem[] fireBreathL;
    [SerializeField] BoxCollider2D damageCol;

    Boss boss;
    public int attackDamage;
    public string attackEffect = "";

    public int _index = 0;
    void Start()
    {
        int i = 0;

        if (attackDamage == 0)
        {
            attackDamage = 20;
        }
        boss = GetComponentInParent<Boss>();
        animator = GetComponent<Animator>();

    }
    void shootFireBall()
    {
        foreach (ParticleSystem _fireBreath in fireBreathR)
        {
            _fireBreath.Play();
        }

        //if (boss.entityDir >= 0)
        //{
        //    foreach (ParticleSystem _fireBreath in fireBreathR)
        //    {
        //        _fireBreath.Play();
        //    }
        //}
        //else
        //{
        //    foreach (ParticleSystem _fireBreath in fireBreathL)
        //    {
        //        _fireBreath.Play();
        //    }
        //}

    }

    void ColliderOn()
    {
        damageCol.enabled = true;
        //if (boxCollider[_index] != null)
        //{
        //    boss.ZeroVelocityX();
        //    boxCollider[_index].enabled = true;
        //}
        //else
        //{
        //    Debug.Log("boxCollider is Null");
        //}
        Debug.Log("ColOn");
    }
    void ColliderOff()
    {
        damageCol.enabled = false;

        //if (boxCollider[_index] != null)
        //{
        //    boxCollider[_index].enabled = false;
        //}
        //else
        //{
        //    Debug.Log("boxCollider is Null");
        //}
        Debug.Log("ColOff");

    }
    public void AnimationTrigger()
    {
        boss.animationTrigger = true;
    }
    void ChasePlayer()
    {
        if (boss.AbleAttackCheck())
        {
            boss.animationTrigger = true;

        }
    }

    void BackStep()
    {
        boss.BossBackStep();
    }
    void Earthquake()
    {
        boss.Earthquake();
    }
}
