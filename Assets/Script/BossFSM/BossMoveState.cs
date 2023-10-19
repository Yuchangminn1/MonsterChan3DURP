using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Rendering;

public class BossMoveState : BossState
{
    protected bool isMoving;
    protected bool isIdle;
    float changeTime = 5f;
    float EnterTimer = 0f;

    public BossMoveState(Boss _boss, int _currentStateNum) : base(_boss, _currentStateNum)
    {
        boss = _boss;
        currentStateNum = _currentStateNum;
    }

    public override void Enter()
    {
        base.Enter();
        EnterTimer = 0f;
        if (boss.isSleep)
        {
            boss.SetState2(0);
            boss.ableAttack = false;
            return;
        }
        else if (boss.isScream)
        {
            boss.SetState2(4);
            boss.isScream = false;
            boss.animationTrigger = false;
            return;

        }
        else
        {
            boss.SetState2(1);
            boss.ableAttack = true;
            return;
        }
    }
    public override void Update()
    {
        base.Update();
        if (boss.stateNum2 == 4 && boss.animationTrigger)
        {
            boss.SetState2(5);
            boss.ableAttack = true;

        }
        if (boss.stateNum2 == 0 || boss.stateNum2 == 4)
        {
            return;
        }
        IdleOrMoving();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (boss.stateNum2 == 0 || boss.stateNum2 == 4)
        {
            return;
        }
        if (startTime+ boss.attackdealay < Time.time && boss.ableAttack)
        {
            Debug.Log("AttackChange");
            boss.StateChange(boss.attackState);
            return;
        }

        //if (boss.ableAttack && boss.AbleAttackCheck())
        //{
        //    boss.StateChange(boss.attackState);
        //    return;
        //}
        if (isMoving)
            boss.Move();
        else
        {
            boss.ZeroHorizontal();
        }
    }


    public override void Exit()
    {
        base.Exit();

    }
    private void IdleOrMoving()
    {
        if (stateTimer > startTime + changeTime || EnterTimer ==0)
        {
            EnterTimer = changeTime;
            startTime = Time.time;
            int ran = Random.Range(1, 2);
            if (ran == 1)
            {
                Move();
            }
            else
            {
                Idle();
            }
        }
    }

    private void Idle()
    {
        //Debug.Log("Idle");
        boss.SetState2(1);
        //boss.ZeroVelocityX();
        isMoving = false;
        isIdle = true;
    }

    private void Move()
    {
        //Debug.Log("Moving");
        boss.SetState2(2);
        isMoving = true;
        isIdle = false;
    }

    //void FF()
    //{
    //    if (boss.AbleAttack())
    //    {
    //        boss.animator.SetInteger("State", 2);
    //        return;
    //    }
    //    else
    //    {
    //        boss.animator.SetInteger("State", 0);
    //        return;

    //    }
    //}
}
