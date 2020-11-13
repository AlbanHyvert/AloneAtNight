﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tp_IdleState : Tp_State
{
    public Tp_IdleState(ThirdPersonMovement player) : base(player)
    {
    }

    public override void Start()
    {

    }

    public override void IsPushing(bool value)
    { 
        if(value == true)
        {
            Player.SetState(new Tp_PushState(Player));
        }
    }

    public override void IsClimbing(bool value)
    {
        if (value == true)
        {
            Player.SetState(new Tp_ClimbState(Player));
        }
    }

    public override void IsThrowing(bool value)
    {

    }
}
