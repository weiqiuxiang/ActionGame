using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerAnimationController
    {
        private readonly Animator animator;

        public PlayerAnimationController(Animator animator)
        {
            this.animator = animator;
        }
    }
}
