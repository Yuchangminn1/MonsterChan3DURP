using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitState : BossState
{
    float groggyTime = 5f;
    public BossHitState(Boss _boss, int _currentStateNum) : base(_boss, _currentStateNum)
    {
        boss = _boss;
        currentStateNum = _currentStateNum;
    }

    public override void Enter()
    {
        base.Enter();
        boss.ableAttack = false;

    }


    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {

        base.FixedUpdate();
        if (boss.animationTrigger)
        {
            
            boss.AnimaPlay("Groggy");
            boss.animationTrigger = false;
            boss.SetInt("State2", 1);
        }
        if (startTime+ groggyTime <= Time.time)
        {
            boss.StateChange(boss.moveState);
            
        }

    }
    public override void Exit()
    {
        base.Exit();
        boss.isGroggy = false;
        boss.ableAttack = true;

    }
    //public virtual void Exit()
    //{
    //    boss.stateNum2 = 0;
    //    boss.animationTrigger = false;
    //}
}
