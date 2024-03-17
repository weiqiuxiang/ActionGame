using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.ActionGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.ActionGame
{
    /// <summary>
    /// 次のステータスにパスするデータ、必要に応じて変数を増やしてください
    /// </summary>
    public class NextStateData
    {
        public Vector3 forward;

        public void Reset()
        {
            forward = Vector3.zero;
        }
    }
    
    /// <summary>
    /// プレーヤーステータスBase
    /// </summary>
    public abstract class PlayerStateBase
    {
        protected readonly PlayerController playerController;
        public NextStateData NextStateData { get; }

        public PlayerStateBase(PlayerController playerController)
        {
            this.playerController = playerController;
            NextStateData = new NextStateData();
        }

        /// <summary>
        /// ステータスに入った時
        /// </summary>
        /// <param name="lastStatus"></param>
        /// <param name="data"></param>
        public virtual void InStatus(PlayerStatus lastStatus, NextStateData data) { }

        /// <summary>
        /// ステータスを抜けた時
        /// </summary>
        public virtual void OutStatus()
        {
            NextStateData.Reset();
        }

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
