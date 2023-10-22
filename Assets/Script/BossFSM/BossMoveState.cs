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
        //���� üũ�ϰ� State2�� ���������ν� �ִϸ��̼� ����
        StateCheck();

        //boss.LookPlayer();

    }
    public override void Update()
    {
        base.Update();
        IdleOrMoving();
        //���⼭ startTime�� ������
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


    //���ο����� ����ϴ� �Լ�
    private void Action()
    {
        boss.Gravity();
        if (ableToBattle)
        {
            //��Ʋ���·� ������ �����Ѱ�
            if (attackDealayTimer < Time.time)
            {
                //���� ������
                boss.StateChange(boss.battleState);
                return;
            }
            //���� üũ �� �̵� 
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
            //�ʹ� �ݺ��̸� �����ϴϱ� ������
            changeTime = Random.Range(5f,10f);
            //���⼭ startTime�� ������
            startTime = Time.time;
            StateCheck();
        }
    }
    private void StateCheck()
    {
        //���� üũ�ϰ� State2�� ���������ν� �ִϸ��̼� ����
        if (boss.isSleep)
        {
            bossIdleState = BossIdleState.Sleep;
        }
        else if (boss.isScream)
        {
            bossIdleState = BossIdleState.Scream;
            boss.isScream = false;
            //�ῡ�� ���� �ٷ� ��Ʋ�� ����� -5 ����
            attackDealayTimer -= 5;
            //�̰� ���̽� ���Ϳ��� ó�����ٰž�
            //boss.animationTrigger = false;
        }
        else
        {
            //�� �� ���°� �ƴ϶�� ���̵� or ��ũ 
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
