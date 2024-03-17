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

        private Dictionary<PlayerStatus, PlayerStateBase> playerStatusDictionary;

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
                playerStatusDictionary = new Dictionary<PlayerStatus, PlayerStateBase>();
                
                // 各state初期化
                playerStatusDictionary.Add(PlayerStatus.IdleAndMove, new PlayerIdleAndMoveState(controller));
                playerStatusDictionary.Add(PlayerStatus.InAir, new PlayerInAirState(controller));
                playerStatusDictionary.Add(PlayerStatus.Dodge, new PlayerDodgeState(controller));
            }
            
            playerStatusData.SetStatus(PlayerStatus.IdleAndMove);
            playerStatusDictionary[playerStatusData.CurrentStatus].InStatus(PlayerStatus.None, null);
        }

        public void FixedUpdateState()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentStatus].FixedUpdate());
        }

        public void UpdateState()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentStatus].Update());
        }

        private void UpdateStatus(PlayerStatus newStatus)
        {
            if (newStatus == PlayerStatus.None) return;
            playerStatusDictionary[playerStatusData.CurrentStatus].OutStatus();
            PlayerStatus lastStatus = playerStatusData.CurrentStatus;
            playerStatusData.SetStatus(newStatus);
            playerStatusDictionary[playerStatusData.CurrentStatus].InStatus(lastStatus, playerStatusDictionary[lastStatus].NextStateData);
        }
    }
}
