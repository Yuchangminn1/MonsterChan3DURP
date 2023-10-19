using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : PlayerState
{
    public GroundState(PlayerController _player, int _currentStateNum) : base(_player, _currentStateNum)
    {
        player = _player;
        currentStateNum = _currentStateNum;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (!player.IsGround())
        {
            player.StateChange(player.fallState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

    
}
