using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Rendering;

public class BossMoveState : BossState
{
    bool ableToBattle = false;
    float changeTime = 5f;
    float attackDealayTimer = 0f;
    //float EnterTimer = 0f;
    enum BossIdleState
    {
        Idle = 0,
        Walk = 1,
        Sleep = 2,
        Scream = 3,
    }
    BossIdleState bossIdleState = BossIdleState.Idle;
    public BossMoveState(Boss _boss, int _currentStateNum) : base(_boss, _currentStateNum)
    {
        boss = _boss;
        currentStateNum = _currentStateNum;
    }

    public override void Enter()
    {
        base.Enter();
        attackDealayTimer = startTime + boss.attackdealay;
        //상태 체크하고 State2를 변경함으로써 애니메이션 변경
        StateCheck();

        //boss.LookPlayer();

    }
    public override void Update()
    {
        base.Update();
        IdleOrMoving();
        //여기서 startTime을 변경함
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Action();
        

    }
    public override void Exit()
    {
        base.Exit();
    }


    //내부에서만 사용하는 함수
    private void Action()
    {
        boss.Gravity();
        if (ableToBattle)
        {
            //배틀상태로 진입이 가능한가
            if (attackDealayTimer < Time.time)
            {
                //공격 딜레이
                boss.StateChange(boss.battleState);
                return;
            }
            //상태 체크 후 이동 
            else if (bossIdleState == BossIdleState.Walk)
            {
                boss.BossChasePlayer();
            }
            else if (bossIdleState == BossIdleState.Idle)
            {
                boss.ZeroVelocity();
            }
            
        }
    }
    private void IdleOrMoving()
    {
        if (stateTimer > startTime + changeTime )
        {
            //너무 반복이면 지루하니까 랜덤값
            changeTime = Random.Range(5f,10f);
            //여기서 startTime을 변경함
            startTime = Time.time;
            StateCheck();
        }
    }
    private void StateCheck()
    {
        //상태 체크하고 State2를 변경함으로써 애니메이션 변경
        if (boss.isSleep)
        {
            bossIdleState = BossIdleState.Sleep;
        }
        else if (boss.isScream)
        {
            bossIdleState = BossIdleState.Scream;
            boss.isScream = false;
            //잠에서 깨면 바로 배틀로 가라고 -5 해줌
            attackDealayTimer -= 5;
            //이건 베이스 엔터에서 처리해줄거야
            //boss.animationTrigger = false;
        }
        else
        {
            //위 두 상태가 아니라면 아이들 or 워크 
            ableToBattle = true;
            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                bossIdleState = BossIdleState.Idle;
            }
            else if (ran == 1)
            {
                bossIdleState = BossIdleState.Walk;
            }
        }
        boss.SetState2((int)bossIdleState);
    }
    
}
