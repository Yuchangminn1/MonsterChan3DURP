using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : EntityState
{
    protected PlayerController player;
    
    protected float airTime;
    

    public PlayerState(PlayerController _player, int _currentStateNum)
    {
        player = _player;
        currentStateNum = _currentStateNum;
    }
    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
        player.SetInt("State", currentStateNum);
        if (currentStateNum != 0) { player.animationTrigger = true; }
    }
    public override void Update()
    {
        base.Update();
        stateTimer = Time.time;
        if(Input.GetKeyDown(KeyCode.X) && isAbleAttack) 
        {
            if (player.ableAirAttack)
            {
                player.StateChange(player.attackState);
                player.ableAirAttack = false;
            }
        }
        if (!isAbleFly)
        {
            if (!player.IsGround())
            {
                if(airTime == 0f)
                {
                    airTime = Time.time;
                }
                if(Time.time - airTime > 0.25f)
                {
                    player.StateChange(player.fallState);

                }
            }
            else
            {
                airTime = 0f;
            }
        }

        BaseState();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        

    }

    public override void Exit()
    {
        base.Exit();

    }
    

    
    protected void BaseState()
    {
        if (!player.animationTrigger && endMotionChange)
        {
            if (player.IsGround()) 
            {
                player.StateChange(player.moveState);
            }
            else
            {
                player.StateChange(player.fallState);
            }
        }
    }


}
