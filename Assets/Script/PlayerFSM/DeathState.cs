using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : PlayerState
{
    public DeathState(PlayerController _player, int _currentStateNum) : base(_player, _currentStateNum)
    {
        player = _player;
        currentStateNum = _currentStateNum;
        endMotionChange = false;
        isAbleFly = true;
        isAbleAttack = false;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
       // base.Update();
        if (!player.animationTrigger)
        {
            //player.ZeroVelocity();
            ;
            //���� UI
            //����Ʈ  �� ��� 
        }
    }
    

    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {

    }

}
