using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// プレイヤーのステータス遷移
    /// </summary>
    public class PlayerStatusManager
    {
        private readonly PlayerAnimationController animationController;
        private readonly PlayerController playerController;
        private readonly PlayerStatusData playerStatusData = new PlayerStatusData();

        private Dictionary<PlayerStatus, PlayerStatusBase> playerStatusDictionary;

        public PlayerStatusManager(PlayerAnimationController animationController, PlayerController playerController)
        {
            this.animationController = animationController;
            this.playerController = playerController;
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
                playerStatusDictionary.Add(PlayerStatus.IdleAndMove, new PlayerIdleAndMoveStatus(animationController, playerController));
            }
            
            playerStatusData.SetStatus(PlayerStatus.IdleAndMove);
            playerStatusDictionary[playerStatusData.CurrentStatus].InStatus(PlayerStatus.None);
        }

        public void FixedUpdate()
        {
            UpdateStatus(playerStatusDictionary[playerStatusData.CurrentStatus].FixedUpdate());
        }

        public void Update()
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
