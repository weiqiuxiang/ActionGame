using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class SetMoveValueZero : StateMachineBehaviour
    {
        private static readonly int MoveValue = Animator.StringToHash("MoveValue");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(MoveValue, 0);
        }
    }
}
