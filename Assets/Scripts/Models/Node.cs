using UnityEngine;

namespace Models
{
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
}