using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public enum PlayerState
    {
        None = 0,
        IdleAndMove, // Idleと移動
        InAir, // 空中
        Attack, // 攻撃中
        Dodge, // 回避中
        Damaged,    // 攻撃受けた後
        NoAction,   // 何も操作できない硬直状態、一定時間後、この状態を抜ける
    }
    
    public enum MoveStatus
    {
        Idle = 0,
        Walk,
        Run,
        Dash
    }
}
