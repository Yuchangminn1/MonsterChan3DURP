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
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (startTime+ groggyTime <= Time.time)
        {
            boss.StateChange(boss.moveState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        boss.isGroggy = false;
    }
    
}
