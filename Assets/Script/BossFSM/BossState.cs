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
        //�̰Ŵ� ������Ʈ �ӽſ��� �� ����ϴ°� �ƴϾ� ?
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
        //������Ʈ 2 �ʱ�ȭ ���߿� ���� ����� ������ �� Ȯ���ϱ�
        boss.SetState2(0);
        
        //boss.animationTrigger = false; Enter���� ó�����༭ ���� �ؾ��ϳ� ?
    }
    
}
