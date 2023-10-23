using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossBattleState : BossState
{
    //���� 
    public bool IsChase { get { return isChase; } }
    //������ ������Ʈ �ȿ����� 
    bool isChase = false;
    
    float playerDis;

    //�������� 
    bool meleeAttack = false;
    //���� �ִϸ��̼� ����
    int attackState2 = 0;
    //�극�� ��Ÿ��
    float breatTime = 0f;
    float breatCoolTime = 10f;
    //�������� 
    BossAttackPattern bossAttackPattern;
    //���ݰŸ�
    BossAttackDis bossAttackDis;
    //���� ������
    BossAttackDamage bossAttackDamage;

    //������ �ݶ��̴� Vector3 2���� �ٲ���ҵ� 
    Vector4[] _collider;


    //��������
    //0 None 1 ������ 2 ����ġ�� 3 �ڷ� ���� 4 �극�� 5 ������ ���� �����
    enum BossAttackPattern
    {
        None = 0,
        BasicAttack,
        TailAttack,
        BackStep,
        Breath,
        earthquake,
        chase = 11
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
    public BossBattleState(Boss _boss, int _currentStateNum) : base(_boss, _currentStateNum)
    {
        boss = _boss;
        currentStateNum = _currentStateNum;
        bossAttackPattern = BossAttackPattern.None;
        bossAttackDis = BossAttackDis.None;
        bossAttackDamage = BossAttackDamage.None;
    }
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Boss Attack Enter");
        //���� ������Ʈ�� ������ ���ӸŴ������� �������� �غ���
        playerDis = GameManager.instance.DistanceToPlayer(boss.transform.position);
        if (bossAttackPattern == 0)
        {
            BossAttack(playerDis);
        }
        boss.SetDamageCol((int)bossAttackPattern);
    }

    public override void Update()
    {
        base.Update();
        boss.LookPlayer();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //�÷��̾� �Ĵٺ���
        //boss.LookPlayer();

        AttackCheck();
        //���ݸ���� �������� �ʱ�ȭ ���� ���̵�� �ؾ��ҵ�
        if (boss.animationTrigger)
        {
            bossAttackPattern = 0;
            boss.SetState2((int)bossAttackPattern);
            boss.StateChange(boss.moveState);
        }


    }
    public override void Exit()
    {
        Debug.Log("Attack Exit");
        base.Exit();
    }
    private void AttackCheck()
    {
        //������ ������������ ������ �� �ִ� ���� �ȿ� �÷��̾ �ִ���
        //������ ���� 
        //�ʹ� �� ��� ���� �ٽ� ����
        if (isChase)
        {
            playerDis = GameManager.instance.DistanceToPlayer(boss.transform.position);

            Debug.Log("boss.ableAttackDis = " + boss.ableAttackDis + "playerDis" + playerDis);

            //�߰��� ���ݹ��� ������ ���Դ°�?
            if (boss.ableAttackDis >= playerDis)
            {
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
            else if (playerDis > 15f && meleeAttack)
            {
                //�ʹ� �ָ� �������� ���� 
                bossAttackPattern = 0;
                boss.SetState2((int)bossAttackPattern);
                boss.StateChange(boss.moveState);
            }
            else
            {
                //����
                boss.BossChasePlayer();
            }
        }
    }

    

    private void BossAttack(float playerDis)
    {
        Debug.Log("���� �÷��̾���� �Ÿ���  " + playerDis);
        if (playerDis <= 20f)
        {
            meleeAttack = true;
            int _value = Random.Range(1, 8);// 0 None   1 ������ 2 ����ġ�� 3 �ڷ� ���� 4 �극�� 5 ������ ���� �����
            if (_value >= 1 && _value < 4)
            {
                bossAttackPattern = BossAttackPattern.BasicAttack;
                bossAttackDis = BossAttackDis.BasicAttack;
                bossAttackDamage = BossAttackDamage.BasicAttack;
            }
            
            else if (_value == 7)
            {
                bossAttackPattern = BossAttackPattern.BackStep;
                bossAttackDis = BossAttackDis.BackStep;
                bossAttackDamage = BossAttackDamage.None;
            }
            else if (_value >= 4 && _value < 7)
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
            if (boss.ableAttackDis >= playerDis)
            {
                //�ٷ� ���� �����Ѱ� ?
                //�̰� ���⼭ �����ؾ��ϴ����� �ǹ��ε� �ϴ� �Ѿ
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
            
            else
            {
                //�ٷ� ������ �� ������ �����Ѵ� 
                //chase�� ���� ������ ������ ���¿��� ���󰡴� ���¸� �ǹ��� 
                isChase = true;
            }
        }
        else
        {
            meleeAttack = false;
            //�극�� ���
            if (breatTime + breatCoolTime < Time.time && playerDis < 21f)
            {
                //�극�� ��Ÿ�� 
                breatTime = Time.time;
                bossAttackPattern = BossAttackPattern.Breath;
                bossAttackDis = BossAttackDis.Breath;
                bossAttackDamage = BossAttackDamage.Breath;
                
            }
            else
            {
                //������ ���� ����� �ϴ� �������� ���� ���Ŀ� ���� �� ��������
                bossAttackPattern = BossAttackPattern.earthquake;
                bossAttackDis = BossAttackDis.earthquake;
                bossAttackDamage = BossAttackDamage.earthquake;
            }
            boss.SetDamage((int)bossAttackDamage);
            boss.SetState2((int)bossAttackPattern);
        }
    }
}
