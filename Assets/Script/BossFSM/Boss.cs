using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Boss : Enemy
{
    public CharacterController CC;
    //플레이어 정보를 옵저버 패턴으로 매니저 만들어서 관리해볼까 
    //public PlayerController player;
    //FSM
    #region FSM
    public BossMoveState moveState { get; private set; }
    public BossBattleState battleState { get; private set; }
    public BossHitState hitState { get; private set; }

    public BossDeathState deathState { get; private set; }

    #endregion
    //test
    bool isIN = false;
    [SerializeField] BossAnimationTrigger bossAnimationTrigger;
    int bossDamage = 20;
    //public bool isableMove = false;

    public bool isScream = false;
    public bool isSleep = true;
    public float moveSpeed = 5f;
    public float BackStepSpeed = 30f;
    public float EarthquakeSpeed = 30f;
    public float playerDis = 0f;
    public Vector3 moveVec = Vector3.zero;
    [SerializeField] Slider hpBar;
    [SerializeField] Image hpBarImage;
    //[SerializeField] BoxCollider2D boxCollider;

    public Vector4[] _collider;

    protected override void Awake()
    {
        base.Awake();

        #region FSM_Initialize
        moveState = new BossMoveState(this, 0);
        battleState = new BossBattleState(this, 1);
        hitState = new BossHitState(this, 5);
        deathState = new BossDeathState(this, 6);
        #endregion
        _collider = new Vector4[6];
        _collider[0] = Vector4.one;

        //basicAttack
        _collider[1] = new Vector4(4f, 5f, 4f, 2.3f);
        //TailAttack
        _collider[2] = new Vector4(10f, 3f, 2f, 0.8f);
        //BackStep
        _collider[3] = new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
        //Breath
        _collider[4] = new Vector4(20f, 6f, 12f, 2.5f);
        //Earth뭐시기
        _collider[5] = new Vector4(1f, 1f, 1f, 1f);
        moveSpeed *= Time.deltaTime;
        BackStepSpeed *= Time.deltaTime;
        EarthquakeSpeed *= Time.deltaTime;
    }

    protected override void Start()
    {
        base.Start();
        CC = GetComponent<CharacterController>();
        if(CC == null)
        {
            Debug.Log("CC is Null");
        }
        if (ableAttackDis == 0)
        {
            ableAttackDis = 15f;

        }
        StateChange(moveState);
        //Flip();
        

    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.G))
        {
            isIN = true;
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerDis = GameManager.instance.DistanceToPlayer(transform.position);
        if (isIN)
        {
            BossBackStep();
            isIN  = false;
        }
    }
    public override void Hit(int _atk, int _groggy = 0)
    {
        base.Hit(_atk, _groggy);
        //UI에 체력 표시 
        HpBarValue(hp, hpMax);
        if (isGroggy)
        {
            StateChange(hitState);
            return;
        }

        if (isdead)
        {
            StateChange(deathState);
            Destroy(gameObject, 1.5f);
            return;
        }
        if (isSleep)
        {
            isScream = true;
            isSleep = false;
            LookPlayer();
            StateChange(moveState);
            return;
        }
        
    }
    #region RayCastCheck
    public bool IsGround() => IsGround(transform);
    public bool DirIsGround() => DirIsGround(transform);

    public bool AbleAttackCheck() => Physics.Raycast(transform.position, Vector3.forward, ableAttackDis, playerLayer);

    //protected override void OnDrawGizmos()
    //{
    //    base.OnDrawGizmos();
    //    Gizmos.DrawRay(transform.position, Vector3.forward * ableAttackDis);
    //}

    #endregion



    //public void DamageBox(Vector4 tmp)
    //{
    //    //박스콜라이더 수정
    //    boxCollider.offset = new Vector2(tmp.z, tmp.w);
    //    boxCollider.size = new Vector2(tmp.x, tmp.y);
    //}
    //public float TargetDis()
    //{
    //    Vector2 tmp = transform.position;
    //    Vector2 tmp1 = player.transform.position;
    //    return Vector2.Distance(tmp, tmp1);
    //}
    //public float TargetDisX()
    //{
    //    float tmp = transform.position.x;
    //    float tmp1 = player.transform.position.x;
    //    return Mathf.Abs(tmp - tmp1);
    //}
    void HpBarValue(int _hp, int _hpMax)
    {
        hpBar.value = (float)_hp / _hpMax;
        if (hpBar.value <= 0.01)
        {
            hpBarImage.enabled = false;
            //StateChange(hitState);
            UIScript.instance.UIOnOff(0, false);
            //Destroy(gameObject, 1.5f);
        }
    }

    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Gard")
    //    {
    //        Hit(gard.gardDamage, gard.gardDamage);
    //        UIScript.instance.PlayerEffect(1);
    //    }
    //}
    #region RigidBody
    public Vector3 ToPlayerVec() => moveVec = (player.transform.position - transform.position).normalized;
   
    public void ZeroVelocity()
    {
        if (isStop || playerDis<1f) return;
        CC.Move(Vector3.zero);
    }
    public void BossChasePlayer()
    {


        //Velocity.y 조절 필요 
        //moveSpeed =5 > 대충 달리기 느낌스

        LookPlayer();
        if (isStop || playerDis < 4f) return;

        moveVec = ToPlayerVec();
        moveVec.y = 0f;
        CC.Move(moveVec * moveSpeed);

    }
    public void LookPlayer()
    {
        //if() { }
        transform.LookAt(player.transform.position);
    }
    public void Earthquake()
    {
        if (isStop) return;

        ZeroVelocity();
        transform.LookAt(player.transform.position);
        //돌진
        moveVec = ToPlayerVec();
        moveVec.y = 0f;
        Vector3 ActionVec = transform.forward * (GameManager.instance.DistanceToPlayer(transform.position)-4.5f);
        StartCoroutine(moveFixedUpdate(ActionVec));
        
    }
    public void BossBackStep()
    {
        if (isStop) return;
        transform.LookAt(player.transform.position);
        ZeroVelocity();
        moveVec = transform.forward;
        moveVec.y = 0f;
        Vector3 ActionVec = moveVec * -BackStepSpeed;
        StartCoroutine(moveFixedUpdate(ActionVec* 50f));
    }
    IEnumerator moveFixedUpdate(Vector3 _Vector)
    {
        float _i = 0;
        float _y = 0.2f;
        float divNum = 50;
        while (_i< divNum)
        {
            if(_i < divNum / 2)
            {
                _Vector.y = _y;
            }
            else
            {
                _Vector.y = -_y;
            }
            CC.Move(_Vector/ divNum);
            _i++;
            yield return new FixedUpdate();
        }
        

    }
    //public void Move()
    //{
    //    dis = TargetDis();
    //    if (dis > 5f)
    //    {
    //        SetState2(2);

    //        rb.velocity = entityDir * moveSpeed * Time.deltaTime;

    //        animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
    //    }
    //    else
    //    {
    //        SetState2(1);
    //    }
    //}
    //public void Earthquake()
    //{
    //    //ZeroVelocityX();
    //    rb.mass = 10f;
    //    rb.AddForce(entityDir, ForceMode.Impulse);
    //    rb.mass = 10f;

    //}
    //public void BossBackStep()
    //{
    //    //ZeroVelocityX();
    //    rb.mass = 10f;
    //    Debug.Log("entityDir = " + entityDir);
    //    rb.AddForce(entityDir, ForceMode.Impulse);
    //    rb.mass = 100f;
    //}
    #endregion
    public void SetDamage(int _num, string _attackEffect = "")
    {
        //상태이상이 있는가 ?
        bossAnimationTrigger.attackEffect = _attackEffect;
        //데미지
        bossAnimationTrigger.attackDamage = _num;
        //콜라이더
        bossAnimationTrigger._index = _num;
    }
}
