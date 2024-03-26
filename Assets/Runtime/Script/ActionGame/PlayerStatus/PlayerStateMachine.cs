using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// プレイヤーのステータス遷移
    /// </summary>
    public class PlayerStateMachine
    {
        private readonly PlayerController controller;
        private readonly PlayerStatusData playerStatusData = new PlayerStatusData();

        private Dictionary<PlayerState, PlayerStateBase> playerStatusDictionary;

        public PlayerStateMachine(PlayerController controller)
        {
            this.controller = controller;
        }
        
        public void Initialize()
        {
            playerStatusData.Initialize();
            InitializeStatus();
        }

        private void InitializeStatus()
        {
            if (playerStatusDictionary == null)
            {
                playerStatusDictionary = new Dictionary<PlayerState, PlayerStateBase>();
                
                // 各state初期化
                playerStatusDictionary.Add(PlayerState.IdleAndMove, new PlayerIdleAndMoveState(controller));
                playerStatusDictionary.Add(PlayerState.InAir, new PlayerInAirState(controller));
                playerStatusDictionary.Add(PlayerState.Dodge, new PlayerDodgeState(controller));
                playerStatusDictionary.Add(PlayerState.Attack, new PlayerAttackState(controller));
                playerStatusDictionary.Add(PlayerState.NoAction, new PlayerNoActionState(controller));
                playerStatusDictionary.Add(PlayerState.Damaged, new PlayerDamagedState(controller));
            }
            
            playerStatusData.SetStatus(PlayerState.IdleAndMove);
            playerStatusDictionary[playerStatusData.CurrentState].InStatus(PlayerState.None, null);
        }

        public void FixedUpdateState()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentState].FixedUpdate());
        }

        public void UpdateState()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentState].Update());
        }

        private void UpdateStatus(PlayerState newState)
        {
            if (newState == PlayerState.None) return;
            playerStatusDictionary[playerStatusData.CurrentState].OutStatus();
            PlayerState lastState = playerStatusData.CurrentState;
            playerStatusData.SetStatus(newState);
            playerStatusDictionary[playerStatusData.CurrentState].InStatus(lastState, playerStatusDictionary[lastState].NextStateData);
            playerStatusDictionary[lastState].NextStateData.Reset();  // 遷移先にデータを渡す後、リセットする
        }
    }
}
