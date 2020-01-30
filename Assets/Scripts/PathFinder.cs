using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private List<Vector2> m_PathToTarget;
    private List<Node> m_CheckedNodes = new List<Node>();
    private List<Node> m_WaitingNodes = new List<Node>();
    public LayerMask solidLayer;

    public List<Vector2> GetPath(Vector2 target)
    {
        m_CheckedNodes = new List<Node>();
        m_WaitingNodes = new List<Node>();
        m_PathToTarget = new List<Vector2>();
        var startPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        var targetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        if (startPosition == targetPosition) return m_PathToTarget;

        var startNode = new Node(0, startPosition, targetPosition, null);

        m_CheckedNodes.Add(startNode);
        m_WaitingNodes.AddRange(GetNeighbors(startNode));

        while (m_WaitingNodes.Count > 0)
        {
            var nodeToCheck = m_WaitingNodes.FirstOrDefault(node => node.CoastF == m_WaitingNodes.Min(n => n.CoastF));
            if (nodeToCheck.Position == targetPosition) return CalculatePathFromNode(nodeToCheck);

            var walkable = Physics2D.OverlapCircle(nodeToCheck.Position, 0.1f, solidLayer) == null;
            if (!walkable)
            {
                m_WaitingNodes.Remove(nodeToCheck);
                m_CheckedNodes.Add(nodeToCheck);
            }
            else
            {
                m_WaitingNodes.Remove(nodeToCheck);
                if (m_CheckedNodes.Any(node => node.Position == nodeToCheck.Position)) continue;
                AddToCheckedList(nodeToCheck);
                m_WaitingNodes.AddRange(GetNeighbors(nodeToCheck));
            }
        }

        return m_PathToTarget;
    }

    private void AddToCheckedList(Node node)
    {
        var existNode = m_CheckedNodes.Where(n => n.Position == node.Position);
        if (existNode.Any())
        {
            foreach (var n in existNode) m_CheckedNodes.Remove(n);
            var bestNode = existNode.Append(node).Single(n => n.CoastF == existNode.Min(ex => ex.CoastF));
            m_CheckedNodes.Add(bestNode);
        }else
            m_CheckedNodes.Add(node);
    }

    private List<Vector2> CalculatePathFromNode(Node node)
    {
        var currentNode = node;
        var path = new List<Vector2>();
        
        while (currentNode.PreviousNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    private IEnumerable<Node> GetNeighbors(Node parent) =>
        new List<Node>
        {
            
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x + 1, parent.Position.y), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x - 1, parent.Position.y), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x, parent.Position.y + 1), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x, parent.Position.y - 1), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x - 1, parent.Position.y - 1), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x -1, parent.Position.y + 1), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x + 1, parent.Position.y - 1), parent.TargetPosition, parent),
            new Node(parent.CoastFromStartG + 1f, new Vector2(parent.Position.x + 1, parent.Position.y + 1), parent.TargetPosition, parent)
        };
}

public class Node
{
    public float CoastFromStartG { get; set; }
    public float CoastToTargetH { get; set; }
    public float CoastF { get; set; }

    public Vector2 Position { get; set; }
    public Vector2 TargetPosition { get; set; }
    public Node PreviousNode { get; set; }
    
    
    public Node(float coastFromStartG, Vector2 position, Vector2 targetPosition, Node previousNode)
    {
        CoastFromStartG = coastFromStartG;
        Position = position;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        CoastToTargetH = (targetPosition - position).magnitude; //(int)Mathf.Abs(targetPosition.x - position.x) + (int)Mathf.Abs(targetPosition.y - position.y); 
        CoastF = coastFromStartG + CoastToTargetH;
    }
}


