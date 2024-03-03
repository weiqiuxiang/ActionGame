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
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;
        private readonly PlayerStatusData playerStatusData = new PlayerStatusData();

        private Dictionary<PlayerStatus, PlayerStatusBase> playerStatusDictionary;

        private void Start()
        {
            Initialize();
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
                playerStatusDictionary = new Dictionary<PlayerStatus, PlayerStatusBase>();
                
                // 各state初期化
                playerStatusDictionary.Add(PlayerStatus.IdleAndMove, new PlayerIdleAndMoveStatus(controller));
                playerStatusDictionary.Add(PlayerStatus.InAir, new PlayerInAirStatus(controller));
            }
            
            playerStatusData.SetStatus(PlayerStatus.IdleAndMove);
            playerStatusDictionary[playerStatusData.CurrentStatus].InStatus(PlayerStatus.None);
        }

        private void FixedUpdate()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentStatus].FixedUpdate());
        }

        private void Update()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentStatus].Update());
        }

        private void UpdateStatus(PlayerStatus newStatus)
        {
            if (newStatus == PlayerStatus.None) return;
            playerStatusDictionary[playerStatusData.CurrentStatus].OutStatus();
            PlayerStatus lastStatus = playerStatusData.CurrentStatus;
            playerStatusData.SetStatus(newStatus);
            playerStatusDictionary[playerStatusData.CurrentStatus].InStatus(lastStatus);
        }
    }
}
