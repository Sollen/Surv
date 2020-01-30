using System;
using Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class AnimationController : MonoBehaviour
    {
        [FormerlySerializedAs("m_MoveState")] 
        public MoveState moveState;
        [FormerlySerializedAs("m_DirectionState")] 
        public DirectionState directionState;
    
        private Animator m_AnimatorController;

        private void Start()
        {
            moveState = MoveState.Idle;
            directionState = DirectionState.Down;
            m_AnimatorController = m_AnimatorController == null ? GetComponent<Animator>() : m_AnimatorController;
            if(m_AnimatorController == null)
                Debug.LogError("Animator is not set to controller");
        }

        private void Update()
        {
            if (moveState != MoveState.Walk)
            {
                m_AnimatorController.Play("Idle");
                return;
            }

            switch (directionState)
            {
                case DirectionState.Right:
                    m_AnimatorController.Play("WalkRight");
                    break;
                case DirectionState.Left:
                    m_AnimatorController.Play("WalkLeft");
                    break;
                case DirectionState.Up:
                    m_AnimatorController.Play("WalkUp");
                    break;
                case DirectionState.Down:
                    m_AnimatorController.Play("WalkDown");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}