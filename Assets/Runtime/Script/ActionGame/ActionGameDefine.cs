using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public enum PlayerStatus
    {
        None = 0,
        IdleAndMove, // Idleと移動
        InAir, // 空中
        Attack, // 攻撃中
        Dodge, // 回避中
        Damaged,    // 攻撃受けた後
    }
    
    public enum MoveStatus
    {
        NoMove = 0,
        Walk,
        Run,
        Dash
    }
}
