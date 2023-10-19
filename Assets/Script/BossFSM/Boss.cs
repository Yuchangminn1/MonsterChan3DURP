using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : Enemy
{
    //FSM
    #region FSM
    public BossMoveState moveState { get; private set; }
    public BossAttackState attackState { get; private set; }
    public BossHitState hitState { get; private set; }

    public BossDeathState deathState { get; private set; }

    #endregion
    [SerializeField] BossAnimationTrigger bossAnimationTrigger;
    int bossDamage = 20;
    public bool isScream = false;
    public bool isSleep = true;
    public float moveSpeed = 5f;
    public float dis = 0f;
    [SerializeField] Slider hpBar;
    [SerializeField] Image hpBarImage;
    [SerializeField] BoxCollider2D boxCollider;

    public Vector4[] _collider;

    protected override void Awake()
    {
        base.Awake();
        #region FSM_Initialize
        moveState = new BossMoveState(this, 0);
        attackState = new BossAttackState(this, 1);
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
        
    }

    protected override void Start()
    {
        base.Start();
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

    }
    protected override void FixedUpdate()
    {

        base.FixedUpdate();
    }
    public override void Hit(int _atk, int _groggy = 0)
    {
        //Debug.Log("Boss _atk ,_groggy" + _atk + "," + _groggy);
        base.Hit(_atk, _groggy);
        HpBarValue(hp, hpMax);
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
            StateChange(moveState);
            return;
        }
        if (iGroggy == iGroggyMax)
        {
            StateChange(hitState);
            iGroggy = 0;
            return;
        }
    }
    #region RayCastCheck
    public bool IsGround() => IsGround(transform);
    public bool DirIsGround() => DirIsGround(transform);

    #endregion

    public void Move()
    {
        dis = TargetDis();

        if (dis > 5f)
        {
            SetState2(2);
            //Debug.Log("dis = " + dis);
            //if (entityDir == -1)
            //{
            //    rb.velocity = new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            //}
            //else
            //{
            //    rb.velocity = new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
            //}
            //Flip();
            rb.velocity = entityDir *moveSpeed * Time.deltaTime;

            animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
        }
        else
        {
            SetState2(1);

        }

    }

    public void DamageBox(Vector4 tmp)
    {
        boxCollider.offset = new Vector2(tmp.z, tmp.w);
        boxCollider.size = new Vector2(tmp.x, tmp.y);
    }
    public float TargetDis()
    {
        Vector2 tmp = transform.position;
        Vector2 tmp1 = player.transform.position;
        return Vector2.Distance(tmp, tmp1);
    }
    public float TargetDisX()
    {
        float tmp = transform.position.x;
        float tmp1 = player.transform.position.x;
        return Mathf.Abs(tmp - tmp1);
    }
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Gard")
        {
            Hit(gard.gardDamage, gard.gardDamage);
            UIScript.instance.PlayerEffect(1);
        }
    }
    public void Earthquake()
    {
        //ZeroVelocityX();
        rb.mass = 10f;
        rb.AddForce(entityDir, ForceMode.Impulse);
        rb.mass = 10f;

    }
    public void BossBackStep()
    {
        //ZeroVelocityX();
        rb.mass = 10f;
        Debug.Log("entityDir = " + entityDir);
        rb.AddForce(entityDir, ForceMode.Impulse);
        rb.mass = 100f;
    }

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
