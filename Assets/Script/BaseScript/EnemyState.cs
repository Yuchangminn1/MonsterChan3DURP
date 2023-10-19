using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState
{
    [SerializeField] protected Enemy enemy;

    public EnemyState(Enemy _enemy, int _currentStateNum)
    {
        enemy = _enemy;
        currentStateNum = _currentStateNum;
    }

    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void Exit()
    {
        base .Exit();
    }
}
