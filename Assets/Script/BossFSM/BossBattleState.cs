using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossBattleState : BossState
{
    //추적 
    public bool IsChase { get { return isChase; } }
    //변경은 스테이트 안에서만 
    bool isChase = false;
    
    float playerDis;

    //근접공격 
    bool meleeAttack = false;
    //공격 애니메이션 변경
    int attackState2 = 0;
    //브레스 쿨타임
    float breatTime = 0f;
    float breatCoolTime = 10f;
    //보스패턴 
    BossAttackPattern bossAttackPattern;
    //공격거리
    BossAttackDis bossAttackDis;
    //공격 데미지
    BossAttackDamage bossAttackDamage;

    //데미지 콜라이더 Vector3 2개로 바꿔야할듯 
    Vector4[] _collider;


    //공격패턴
    //0 None 1 깨물기 2 꼬리치기 3 뒤로 점프 4 브레스 5 앞으로 점프 땅찍기
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
    //공격패턴 사거리
    enum BossAttackDis
    {
        None = 0,
        BasicAttack = 6,
        TailAttack = 10,
        BackStep,
        Breath = 20,
        earthquake
    }
    //공격패턴 데미지
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
        //메인 오브젝트들 정보는 게임매니저에서 가져오게 해보자
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
        //플레이어 쳐다보기
        //boss.LookPlayer();

        AttackCheck();
        //공격모션이 끝났으면 초기화 논을 아이들로 해야할듯
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
        //정해진 공격패턴으로 공격할 수 있는 범위 안에 플레이어가 있는지
        //없으면 추적 
        //너무 멀 경우 패턴 다시 산출
        if (isChase)
        {
            playerDis = GameManager.instance.DistanceToPlayer(boss.transform.position);

            Debug.Log("boss.ableAttackDis = " + boss.ableAttackDis + "playerDis" + playerDis);

            //추격중 공격범위 안으로 들어왔는가?
            if (boss.ableAttackDis >= playerDis)
            {
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
            else if (playerDis > 15f && meleeAttack)
            {
                //너무 멀면 추적하지 말자 
                bossAttackPattern = 0;
                boss.SetState2((int)bossAttackPattern);
                boss.StateChange(boss.moveState);
            }
            else
            {
                //추적
                boss.BossChasePlayer();
            }
        }
    }

    

    private void BossAttack(float playerDis)
    {
        Debug.Log("현재 플레이어와의 거리는  " + playerDis);
        if (playerDis <= 20f)
        {
            meleeAttack = true;
            int _value = Random.Range(1, 8);// 0 None   1 깨물기 2 꼬리치기 3 뒤로 점프 4 브레스 5 앞으로 점프 땅찍기
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
                //꼬리치기는 넉백 사거리 체크 필요없음
                boss.ableAttackDis = (int)bossAttackDis;

                boss.SetDamage((int)bossAttackDamage, "Knockback");
                boss.SetState2((int)bossAttackPattern);
                return;
            }


            boss.ableAttackDis = (int)bossAttackDis;
            if (boss.ableAttackDis >= playerDis)
            {
                //바로 공격 가능한가 ?
                //이걸 여기서 생각해야하는지는 의문인데 일단 넘어감
                boss.SetDamage((int)bossAttackDamage);
                boss.SetState2((int)bossAttackPattern);
                isChase = false;
            }
            
            else
            {
                //바로 공격할 수 없으면 추적한다 
                //chase는 공격 패턴이 정해진 상태에서 따라가는 상태를 의미함 
                isChase = true;
            }
        }
        else
        {
            meleeAttack = false;
            //브레스 쏘기
            if (breatTime + breatCoolTime < Time.time && playerDis < 21f)
            {
                //브레스 쿨타임 
                breatTime = Time.time;
                bossAttackPattern = BossAttackPattern.Breath;
                bossAttackDis = BossAttackDis.Breath;
                bossAttackDamage = BossAttackDamage.Breath;
                
            }
            else
            {
                //앞으로 점프 땅찍기 일단 돌진으로 보임 추후에 수정 할 생각있음
                bossAttackPattern = BossAttackPattern.earthquake;
                bossAttackDis = BossAttackDis.earthquake;
                bossAttackDamage = BossAttackDamage.earthquake;
            }
            boss.SetDamage((int)bossAttackDamage);
            boss.SetState2((int)bossAttackPattern);
        }
    }
}
