using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [FormerlySerializedAs("MoveSpeed")] 
        public float moveSpeed = 1f;
    
        private Vector2? m_Target;
        private Vector2? m_CurrentTarget;
        private List<Vector2> m_PathToTarget;
        private AnimationController m_AnimationController;
   
        private PathFinder m_PathFinder;
        private Rigidbody2D m_Rigidbody;

        private void Start()
        {
            m_AnimationController = m_AnimationController == null ? GetComponent<AnimationController>() : m_AnimationController;
            m_Rigidbody = m_Rigidbody == null ? GetComponent<Rigidbody2D>() : m_Rigidbody;
            m_PathFinder = m_PathFinder == null ? GetComponent<PathFinder>() : m_PathFinder;
            m_PathToTarget = new List<Vector2>();
        
        }

        private void Update()
        {
            if (m_Target == null) return;
       
            if (m_Target != m_CurrentTarget)
            {
                m_CurrentTarget = m_Target;
                m_PathToTarget = m_PathFinder.GetPath(m_CurrentTarget.Value);
            }
        
            if (!m_PathToTarget.Any())
            {
                m_AnimationController.moveState = MoveState.Idle;
                m_Target = null;
                m_CurrentTarget = null;
                return;
            }
        
            m_AnimationController.moveState = MoveState.Walk;
        
            var point = m_PathToTarget[m_PathToTarget.Count - 1];
            var heading = point - (Vector2) transform.position;
            var direction = heading / heading.magnitude;

            if (heading.magnitude > 0.1f)
            {
                ChangeDirection(direction);
                transform.position = Vector2.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
            }
            else
                m_PathToTarget = m_PathFinder.GetPath(m_Target.Value);
        }
    
        private void ChangeDirection(Vector2 direction)
        {
            var x = (int) Math.Round(direction.x);
            var y = (int) Math.Round(direction.y);
            switch (x)
            {
                case 0 when y == 1:
                    m_AnimationController.directionState = DirectionState.Up;
                    break;
                case 0 when y == -1:
                    m_AnimationController.directionState = DirectionState.Down;
                    break;
                case -1 when y == 0:
                case -1 when y == 1:
                case -1 when y == -1:
                    m_AnimationController.directionState = DirectionState.Left;
                    break;
                case 1 when y == 0:
                case 1 when y == 1:
                case 1 when y == -1:
                    m_AnimationController.directionState = DirectionState.Right;
                    break;
                default:
                    m_AnimationController.directionState = DirectionState.Down;
                    break;
            }
        }

        public void ChangeTarget(Vector2 newTarget)
        {
            Debug.Log("New target");
            m_Target = newTarget;
        }

#if DEBUG
        private void OnDrawGizmos()
        {
            if (m_PathToTarget == null) return;
        
            foreach (var item in m_PathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(item, 0.1f);
            }
        }
#endif
    }
}