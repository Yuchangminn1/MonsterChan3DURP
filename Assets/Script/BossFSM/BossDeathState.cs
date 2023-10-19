using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossState
{
    public BossDeathState(Boss _boss, int _currentStateNum) : base(_boss, _currentStateNum)
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
        //base.Update();
    }
    public override void FixedUpdate()
    {
        // base.FixedUpdate();
    }
    public override void Exit()
    {
        //  base.Exit();
        boss.ableAttack = true;

    }



}
