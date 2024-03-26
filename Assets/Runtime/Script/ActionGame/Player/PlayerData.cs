using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerData
    {
        public int Life { get; private set; }
        public int MaxLife { get; private set; }

        public bool IsDead => Life <= 0;
        
        public void Initialize(int life, int maxLife)
        {
            Life = life;
            MaxLife = maxLife;
        }

        public void AddLife(int value)
        {
            Life = Mathf.Clamp(Life + value, 0 ,MaxLife);
        }
    }
    
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    public class PlayerStatusData
    {
        public PlayerState CurrentState { get; private set; }     
        public bool IsGround { get; private set; }  // 着地か？

        public void Initialize()
        {
            IsGround = false;
            CurrentState = PlayerState.IdleAndMove;
        }

        public void SetStatus(PlayerState state)
        {
            CurrentState = state;
        }
        
        public void SetIsGround(bool value)
        {
            IsGround = value;
        }
    }
}
