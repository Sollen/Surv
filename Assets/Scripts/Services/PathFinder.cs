using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Services
{
    public class PathFinder : MonoBehaviour
    {
        private List<Vector2> m_PathToTarget;
        private List<Node> m_CheckedNodes;
        private List<Node> m_WaitingNodes;
        public LayerMask solidLayer;

        public List<Vector2> GetPath(Vector2 target)
        {
            m_CheckedNodes = new List<Node>();
            m_WaitingNodes = new List<Node>();
            m_PathToTarget = new List<Vector2>();
            var position = transform.position;
            var startPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
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
            var existNode = m_CheckedNodes.Where(n => n.Position == node.Position).ToList();
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
}