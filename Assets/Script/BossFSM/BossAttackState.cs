using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossAttackState : BossState
{
    //enum attackCC
    //{
    //    None,
    //    Knockback
    //}
    int attackState2 = 0;
    float breatTime = 0f;
    float breatCoolTime = 10f;
    BossAttackPattern bossAttackPattern;
    BossAttackDis bossAttackDis;
    BossAttackDamage bossAttackDamage;
    public bool isChase = false;
    Vector4[] _collider;
    public BossAttackState(Boss _boss, int _currentStateNum) : base(_boss, _currentStateNum)
    {
        boss = _boss;
        currentStateNum = _currentStateNum;
        bossAttackPattern = BossAttackPattern.None;
        bossAttackDis = BossAttackDis.None;
        bossAttackDamage = BossAttackDamage.None;


    }
    //��������
    enum BossAttackPattern
    {
        None = 0,
        BasicAttack,
        TailAttack,
        BackStep,
        Breath,
        earthquake
    }
    //�������� ��Ÿ�
    enum BossAttackDis
    {
        None = 0,
        BasicAttack = 6,
        TailAttack = 10,
        BackStep,
        Breath = 20,
        earthquake
    }
    //�������� ������
    enum BossAttackDamage
    {
        None = 0,
        BasicAttack = 40,
        TailAttack = 20,
        BackStep = 1,
        Breath = 60,
        earthquake = 30
    }
    public override void Enter()
    {
        base.Enter();
        float playerDisX = boss.TargetDisX();
        //0None   1 ������ 2 ����ġ�� 3 �ڷ� ���� 4 �극�� 5 ������ ���� �����
        if(bossAttackPattern == 0)
        {
            BossAttack(playerDisX);
        }
        boss.DamageBox(boss._collider[(int)bossAttackPattern]);
        Debug.Log("���� ������ " + bossAttackPattern);
    }

   

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isChase)
        {
            //�߰��� ���ݹ��� ������ ���Դ°�?
            if (boss.AbleAttackCheck())
            {
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
            else if (boss.AbleAttackCheckBack())
            {
                //boss.Turn();
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
        }
        if (boss.animationTrigger)
        {
            boss.StateChange(boss.moveState);
            bossAttackPattern = 0;
        }


    }

    public override void Exit()
    {
        Debug.Log("Attack Exit");
        base.Exit();
    }

    private void BossAttack(float playerDis)
    {
        Debug.Log("���� ����");
        Debug.Log("���� �÷��̾���� �Ÿ���  " + playerDis);
        if (playerDis <= 10f)
        {
            int _value = Random.Range(1, 4);// 0 None   1 ������ 2 ����ġ�� 3 �ڷ� ���� 4 �극�� 5 ������ ���� �����
            if (_value == 1)
            {
                bossAttackPattern = BossAttackPattern.BasicAttack;
                bossAttackDis = BossAttackDis.BasicAttack;
                bossAttackDamage = BossAttackDamage.BasicAttack;
            }
            
            else if (_value == 3)
            {
                bossAttackPattern = BossAttackPattern.BackStep;
                bossAttackDis = BossAttackDis.BackStep;
                bossAttackDamage = BossAttackDamage.None;
            }
            else if (_value == 2)
            {
                bossAttackPattern = BossAttackPattern.TailAttack;
                bossAttackDis = BossAttackDis.TailAttack;
                bossAttackDamage = BossAttackDamage.TailAttack;
                //����ġ��� �˹� ��Ÿ� üũ �ʿ����
                boss.ableAttackDis = (int)bossAttackDis;

                boss.SetDamage((int)bossAttackDamage, "Knockback");
                boss.SetState2((int)bossAttackPattern);
                return;
            }


            boss.ableAttackDis = (int)bossAttackDis;
            if (boss.AbleAttackCheck())
            {
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }else if (boss.AbleAttackCheckBack())
            {
                //boss.Turn();
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
            else
            {
                isChase = true;
            }
        }
        else
        {
            if (breatTime + breatCoolTime < Time.time && playerDis < 21f)
            {
                breatTime = Time.time;
                bossAttackPattern = BossAttackPattern.Breath;
                bossAttackDis = BossAttackDis.Breath;
                bossAttackDamage = BossAttackDamage.Breath;
                //�극�� ���
            }
            else
            {
                //������ ���� �����
                bossAttackPattern = BossAttackPattern.earthquake;
                bossAttackDis = BossAttackDis.earthquake;
                bossAttackDamage = BossAttackDamage.earthquake;
            }
            boss.SetDamage((int)bossAttackDamage);
            boss.SetState2((int)bossAttackPattern);
        }
    }
}
