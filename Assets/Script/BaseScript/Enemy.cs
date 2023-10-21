using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

public class Enemy : Entity
{
    //public Rigidbody rb;
    //public CharacterController CC;
    public int iGroggyMax = 100;
    public int iGroggy = 0;
    public float attackdealay = 2f;
    //public bool ableAttack = true;
    public LayerMask playerLayer; // �÷��̾� Ȯ���ϱ� ���� ���̾� ����ũ

    public float ableAttackDis = 0f;

    //[SerializeField] protected  Gard gard;

    protected GameObject player;
    Vector3 originScale;


    protected virtual void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        //rb = GetComponent<Rigidbody>();
        base.Start();
        player = GameObject.Find("Player");
        GroundCheckDis += 0.2f;
        //attackDagame = UIScript.instance.enemyAttackDagame[0];
        originScale.x = transform.localScale.x;
        originScale.y = transform.localScale.y;
        originScale.z =  transform.localScale.z;
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }
    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        
    }
    public virtual void Hit(int _atk, int _groggy = 0)
    {
        //Debug.Log("Enemy _atk ,_groggy" + _atk + "," + _groggy);
        if (hp - _atk > 0)
        {
            hp -= _atk;
            if (!isGroggy)
            {
                GroggyUp(_groggy);
            }
        }
        else
        {
            hp = 0;
            isdead = true;
        }
        
    }
    //public void Turn()
    //{
    //    entityDir *= -1;
    //    transform.localScale = new Vector3(entityDir * originScale.x, originScale.y, originScale.z);
    //}
    //protected virtual void Flip()
    //{
    //    float _goalX = player.transform.position.x;
    //    if (_goalX < transform.position.x)
    //    {
    //        entityDir = -1;
    //    }
    //    else
    //    {
    //        entityDir = 1;
    //    }
    //    transform.localScale = new Vector3(entityDir * originScale.x, originScale.y, originScale.z);
    //}
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawRay(transform.position, new Vector3(1f,1f,1f)  * ableAttackDis);
    }
    public void ResetGroggy()
    {
        isGroggy = true;
        iGroggy = 0;
    }
    public void GroggyUp(int _groggy)
    {
        if (iGroggy + _groggy >= iGroggyMax)
        {
            ResetGroggy();
        }
        else
        {
            iGroggy += _groggy;
        }
    }

    public bool IsGround(Transform transform)
    {
        //�߹�üũ
        return Physics.Raycast(transform.position, Vector3.down, GroundCheckDis, groundLayer);
    }
    public bool DirIsGround(Transform transform)
    {
        //��������� �ٴ��� üũ �� ���� �𷺼����� �޾Ƽ� �÷����ؾ���
        return Physics.Raycast(transform.position, Vector3.down, GroundCheckDis, groundLayer);
    }
    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Gard")
    //    {
    //        Hit(gard.gardDamage, gard.gardDamage);
    //        UIScript.instance.PlayerEffect(1);
    //    }
    //}
}
