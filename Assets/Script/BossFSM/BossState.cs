using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : EntityState
{
    protected Boss boss;

    public BossState(Boss _boss, int _currentStateNum)
    {
        boss = _boss;
        currentStateNum = _currentStateNum;
    }
    public override void Enter()
    {
        base.Enter();
        //이거는 스테이트 머신에서 해 줘야하는거 아니야 ?
        boss.SetInt("State", currentStateNum);
        Debug.Log("currentStateNum = "+ currentStateNum);
        boss.animationTrigger = false;
        //if (currentStateNum != 0) { boss.animationTrigger = false; }
    }
    public override void Update()
    {
        if (!boss.isdead)
        {
            base.Update();
        }
    }

    
    public override void FixedUpdate()
    {
        if (!boss.isdead)
        {
            base.FixedUpdate();
            
        }
    }
    public override void Exit()
    {
        base.Exit();
        //스테이트 2 초기화 나중에 문제 생길수 있으니 잘 확인하기
        boss.SetState2(0);
        
        //boss.animationTrigger = false; Enter에서 처리해줘서 굳이 해야하나 ?
    }
    
}
