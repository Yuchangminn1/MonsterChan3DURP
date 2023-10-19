using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    //체력회복
    //bool heal = false;
    [SerializeField] GameObject gard1;
    public MoveState(PlayerController _player, int _currentStateNum) : base(_player, _currentStateNum)
    {
        player = _player;
        currentStateNum = _currentStateNum;
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpCount = 0;
        player.dodgeCount = 0f;
        player.ableAirAttack = true;
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            player.healNum += UIScript.instance.ResetHPHealNumIcon(1);
            //Debug.Log("DeathStateChange");
            //player.StateChange(player.deathState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.healNum += UIScript.instance.ResetHPHealNumIcon(player.healNumMax);
            //Debug.Log("DeathStateChange");
            //player.StateChange(player.deathState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            player.StateChange(player.jumpState);
            return;
        }
        Debug.Log(player.moveVec);

        if (Input.GetKeyDown(KeyCode.C) && (player.moveVec != Vector3.zero))
        {
            player.StateChange(player.dodgeState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.StateChange(player.healState);
            return;
        }
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.CCMove();

    }



    public override void Exit()
    {
        base.Exit();
    }
}
