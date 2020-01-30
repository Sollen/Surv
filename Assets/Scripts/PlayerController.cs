using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("MoveSpeed")] 
    public float moveSpeed = 5f;
    
    private Vector2? m_Target;
    private Vector2? m_CurrentTarget;
    private List<Vector2> m_PathToTarget;
    private Mover m_Mover;
   
    private PathFinder PathFinder;
    private Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Mover = m_Mover == null ? GetComponent<Mover>() : m_Mover;
        m_Rigidbody = m_Rigidbody == null ? GetComponent<Rigidbody2D>() : m_Rigidbody;
        PathFinder = PathFinder == null ? GetComponent<PathFinder>() : PathFinder;
        m_PathToTarget = new List<Vector2>();
        
    }

    private void Update()
    {
        if (m_Target == null) return;
        if (m_Target != m_CurrentTarget)
        {
            m_PathToTarget = PathFinder.GetPath(m_Target.Value);
            m_CurrentTarget = m_Target;
        }

        if (!m_PathToTarget.Any())
        {
            m_Mover.moveState = MoveState.Idle;
            m_Target = null;
            m_CurrentTarget = null;
            return;
        }

        m_Mover.moveState = MoveState.Walk;
        
        var point = m_PathToTarget[m_PathToTarget.Count - 1];
        var heading = point - (Vector2) transform.position;
        var distance = heading.magnitude;
        var direction = heading / heading.magnitude;
        
        if (distance > 0.1f)
        {
            ChangeDirection(direction);
            transform.position = Vector2.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
        }
        else
            m_PathToTarget = PathFinder.GetPath(m_Target.Value);
    }


    private void OnDrawGizmos()
    {
        if (m_PathToTarget == null) return;
        
        foreach (var item in m_PathToTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(item, 0.1f);
        }
    }

    private void ChangeDirection(Vector2 direction)
    {
        var x = (int) Math.Round(direction.x);
        var y = (int) Math.Round(direction.y);
        switch (x)
        {
            case 0 when y == 1:
                m_Mover.directionState = DirectionState.Up;
                break;
            case 0 when y == -1:
                m_Mover.directionState = DirectionState.Down;
                break;
            case -1 when y == 0:
                m_Mover.directionState = DirectionState.Left;
                break;
            case -1 when y == 1:
                m_Mover.directionState = DirectionState.Left;
                break;
            case -1 when y == -1:
                m_Mover.directionState = DirectionState.Left;
                break;
            case 1 when y == 0:
                m_Mover.directionState = DirectionState.Right;
                break;
            case 1 when y == 1:
                m_Mover.directionState = DirectionState.Right;
                break;
            case 1 when y == -1:
                m_Mover.directionState = DirectionState.Right;
                break;
            default:
                m_Mover.directionState = DirectionState.Down;
                break;
        }
    }

    public void ChangeTarget(Vector2 newTagret)
    {
        m_Target = newTagret;
    }
}