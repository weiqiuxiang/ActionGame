using System.Collections;
using System.Collections.Generic;
using Project.ActionGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.ActionGame
{
    /// <summary>
    /// プレーヤーステータスBase
    /// </summary>
    public abstract class PlayerStatusBase
    {
        protected readonly PlayerController playerController;
        
        public PlayerStatusBase(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        /// <summary>
        /// ステータスに入った時
        /// </summary>
        /// <param name="lastStatus"></param>
        public virtual void InStatus(PlayerStatus lastStatus) { }

        /// <summary>
        /// ステータスを抜けた時
        /// </summary>
        public virtual void OutStatus() { }

        /// <summary>
        /// FixedUpdate用
        /// </summary>
        /// <returns></returns>
        public virtual PlayerStatus FixedUpdate() => PlayerStatus.None;

        /// <summary>
        /// Update用
        /// </summary>
        /// <returns></returns>
        public virtual PlayerStatus Update() => PlayerStatus.None;
    }
}
