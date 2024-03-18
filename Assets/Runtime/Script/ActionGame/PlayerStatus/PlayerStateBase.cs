using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.ActionGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.ActionGame
{
    /// <summary>
    /// ステータス遷移時に渡すデータ
    /// *必要に応じて変数を増やしてください
    /// </summary>
    public class PlayerStateData
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
        public PlayerStateData NextStateData { get; }

        public PlayerStateBase(PlayerController playerController)
        {
            this.playerController = playerController;
            NextStateData = new PlayerStateData();
        }

        /// <summary>
        /// ステータスに入った時
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="receiveData"></param>
        public virtual void InStatus(PlayerState previousState, PlayerStateData receiveData) { }

        /// <summary>
        /// ステータスを抜けた時
        /// </summary>
        public virtual void OutStatus()
        {
            
        }

        /// <summary>
        /// FixedUpdate用
        /// </summary>
        /// <returns></returns>
        public virtual PlayerState FixedUpdate() => PlayerState.None;

        /// <summary>
        /// Update用
        /// </summary>
        /// <returns></returns>
        public virtual PlayerState Update() => PlayerState.None;
    }
}
