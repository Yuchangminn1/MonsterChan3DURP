using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineOrbitalTransposer;

public class PlayerController : Entity
{
    public CharacterController cc;
    //인풋
    //public bool isInput = false;
    //카메라 방향으로 회전 가능 여부
    public bool ablerotate = true;
    [SerializeField] Transform cameraTransform;

    //회피
    public bool isDodge = false;
    public float dodgeCount = 0f;

    //더블점프
    public int jumpCount = 0;
    public bool doubleJump = false;
    public float gravity;
    //공중공격 한번만
    public bool ableAirAttack = true;
    //체력회복 
    public bool isHeal = false;
    public int fHpHeal = 40;
    public int healNum;
    public int healNumMax = 3;
    #region FSM

    public MoveState moveState { get; private set; }
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public LandState landState { get; private set; }
    public AttackState attackState { get; private set; }
    public HitState hitState { get; private set; }
    public DodgeState dodgeState { get; private set; }
    public DeathState deathState { get; private set; }
    public HealState healState { get; private set; }


    #endregion

    [Header("Player Move")]
    public Vector3 moveVec = Vector3.zero;
    [SerializeField] protected float inputX = 0f;
    [SerializeField] protected float inputZ = 0f;

    [SerializeField] protected float moveSpeed = 4f;
    [SerializeField] protected float dodgeSpeed = 7f;
    public float jumpForce = 5f;

    
    [SerializeField] GameObject groundChheckObject;
    //무기 교체
    public GameObject weapon;
    public Transform weaponPos;

    private void Awake()
    {
        base.Awake();
        #region FSM_Initialize
        moveState = new MoveState(this, 0);
        jumpState = new JumpState(this, 1);
        fallState = new FallState(this, 2);
        landState = new LandState(this, 3);
        attackState = new AttackState(this, 4);
        hitState = new HitState(this, 5);
        dodgeState = new DodgeState(this, 6);
        deathState = new DeathState(this, 7);
        healState = new HealState(this, 8);
        #endregion
        healNum = healNumMax;
    }

    protected override void Start()
    {
        base.Start();
        cc = GetComponent<CharacterController>();
        groundChheckObject = GameObject.Find("GroundCheck");
        stateMachine.ChangeState(moveState);
        //이건 나중에 바꿔야지
        //attackDagame = UIScript.instance.playerAttackDagame[0];
        ChangeWeapon(weapon);
    }

    
    protected void Update()
    {
        stateMachine.Update();
        PlayerInput();
    }

    protected void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    protected override void OnDrawGizmos()
    {
        //if (groundChheckObject != null)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawRay(groundChheckObject.transform.position, Vector2.down * GroundCheckDis);
        //    Gizmos.DrawRay(new Vector2(groundChheckObject.transform.position.x + entityDir, transform.position.y - 0.4f), Vector2.down * GroundCheckDis);
        //}
    }
    public void Hit(Collider2D collision)
    {
        if (!isDodge)
        {
            BossAnimationTrigger bossanima = collision.gameObject.GetComponentInParent<BossAnimationTrigger>();
            
            if (bossanima.attackDamage == 0) return;

            base.Hit(bossanima.attackDamage);
            if (isdead)
            {
                StateChange(deathState);
                UIScript.instance.HpBarValue(Hp, HpMax);
                return;
            }
            else if (bossanima.attackEffect == "Knockback")
            {
                PlayerKnockback(collision);
            }
            UIScript.instance.HpBarValue(Hp, HpMax);
            StateChange(hitState);
        }
        
    }

    public bool IsGround() => IsGround(groundChheckObject.transform);

    public bool DirIsGround() => DirIsGround(groundChheckObject.transform);

    public void PlayerInput()
    {
        
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        //inputVec = new Vector3(inputX, 0f, inputZ);
        
        //entityDir = new Vector3(inputX, 0f, inputZ);
        //Flip();
        SetFloat("InputX", inputX);
        SetFloat("InputZ", inputZ);
        moveVec = (transform.forward * inputZ + transform.right * inputX).normalized;

    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.tag == "Enemy")
    //    {
    //        Hit(UIScript.instance.enemyAttackDagame[0]);
    //    }
    //    else if (collision.tag == "Boss")
    //    {
    //        Hit(collision);
    //    }

    //}

    public void PlayerKnockback(Collider2D collision)
    {
        float _bossAttackDis;
        if (collision.transform.position.x > transform.position.x)
        {
            _bossAttackDis = -1f;
        }
        else
        {
            _bossAttackDis = 1f;
        }
       // rb.AddForce(new Vector2(40f * _bossAttackDis, 1f), ForceMode2D.Impulse);
    }


    #region CharacterController
    //Character Controller

    public void CCMove()
    {
        if (ablerotate && moveVec != Vector3.zero )
        {
            Quaternion tmp = new Quaternion(0f, cameraTransform.rotation.y, 0f, cameraTransform.rotation.w);
            transform.rotation = tmp;
        }
        if (inputZ < 0.1f)
        {
            
            cc.Move(moveVec * Time.deltaTime * moveSpeed/2);
            return;
        }
        cc.Move(moveVec * Time.deltaTime*moveSpeed);
    }
    public void CCJump()
    {
        cc.Move( (moveVec + (Vector3.up*jumpForce) ) * Time.deltaTime * moveSpeed);
    }
    
    public void CCDodge(Vector3 _dodgeDir)
    {
        cc.Move(_dodgeDir * Time.deltaTime * dodgeSpeed);
    }
    public void CCStop()
    {
        cc.Move(new Vector3(0f,cc.velocity.y,0f) );
    }
    public void CCGravity(float _gravity)
    {
        Vector3 tmp = Vector3.zero;
        tmp.y = -_gravity;
        cc.Move((cc.velocity.normalized + tmp) * Time.deltaTime);
    }

    #endregion
    public void ChangeWeapon(GameObject _gameObject)
    {
        GameObject _newWeapon = Instantiate(_gameObject, weaponPos);
    }
}
